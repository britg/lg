using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MoveSerializer : LGMonoBehaviour {

	public double interpolationBackTime = 0.2;
	public double extrapolationLimit = 0.5;

	private struct State
	{
		public double timestamp;
		public Vector3 pos;
		public Vector3 vel;
		public Quaternion rot;
	}
	
	public struct Move : IComparable<Move>
	{
		public double timestamp;
		public float deltaTime;
		public Vector3 vel;
		
		public static bool operator ==(Move lhs, Move rhs) { return lhs.timestamp == rhs.timestamp; }
		public static bool operator !=(Move lhs, Move rhs) { return lhs.timestamp != rhs.timestamp; }
		public static bool operator >=(Move lhs, Move rhs) { return lhs.timestamp >= rhs.timestamp; }
		public static bool operator <=(Move lhs, Move rhs) { return lhs.timestamp <= rhs.timestamp; }
		public static bool operator >(Move lhs, Move rhs) { return lhs.timestamp > rhs.timestamp; }
		public static bool operator <(Move lhs, Move rhs) { return lhs.timestamp < rhs.timestamp; }
		
		public override bool Equals(object other)
		{
			if (other == null || !(other is Move))
				return false;
			
			return this == (Move)other;
		}
		
		public override int GetHashCode()
		{
			return timestamp.GetHashCode();
		}
		
		public int CompareTo(Move other)
		{
			if (this > other)
				return 1;
			
			if (this < other)
				return -1;
			
			return 0;
		}
	}

	private CharacterController character;
	State[] proxyStates = new State[20];
	public int proxyStateCount;
	
	public List<Move> ownerMoves = new List<Move>();

	void Awake() {
		character = GetComponent<CharacterController>();
	}

	void uLink_OnSerializeNetworkView(uLink.BitStream stream, uLink.NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			Vector3 pos = transform.position;
			Quaternion rot = transform.rotation;
			Vector3 velocity = character.velocity;
			
			stream.Serialize(ref pos);
			stream.Serialize(ref velocity);
			stream.Serialize(ref rot);
		}
		else
		{
			Vector3 pos = Vector3.zero;
			Vector3 velocity = Vector3.zero;
			Quaternion rot = Quaternion.identity;
			
			stream.Serialize(ref pos);
			stream.Serialize(ref velocity);
			stream.Serialize(ref rot);
			
			// Shift the buffer sideways, deleting state 20
			for (int i = proxyStates.Length - 1; i >= 1; i--)
			{
				proxyStates[i] = proxyStates[i - 1];
			}
			
			
			// Record current state in slot 0
			State state;
			state.timestamp = info.timestamp;
			
			state.pos = pos;
			state.vel = velocity;
			state.rot = rot;
			proxyStates[0] = state;
			
			// Update used slot count, however never exceed the buffer size
			// Slots aren't actually freed so this just makes sure the buffer is
			// filled up and that uninitalized slots aren't used.
			proxyStateCount = Mathf.Min(proxyStateCount + 1, proxyStates.Length);
			
			// Check if states are in order
			if (proxyStates[0].timestamp < proxyStates[1].timestamp)
				Debug.LogError("Timestamp inconsistent: " + proxyStates[0].timestamp + " should be greater than " + proxyStates[1].timestamp);
		}
	}

	void Update() {
		// This is the target playback time of the rigid body
		double interpolationTime = uLink.Network.time - interpolationBackTime;
		
		// Use interpolation if the target playback time is present in the buffer
		if (proxyStates[0].timestamp > interpolationTime)
		{
			// Go through buffer and find correct state to play back
			for (int i=0;i<proxyStateCount;i++)
			{
				if (proxyStates[i].timestamp <= interpolationTime || i == proxyStateCount-1)
				{
					// The state one slot newer (<100ms) than the best playback state
					State rhs = proxyStates[Mathf.Max(i-1, 0)];
					// The best playback state (closest to 100 ms old (default time))
					State lhs = proxyStates[i];
					
					// Use the time between the two slots to determine if interpolation is necessary
					double length = rhs.timestamp - lhs.timestamp;
					float t = 0.0F;
					// As the time difference gets closer to 100 ms t gets closer to 1 in 
					// which case rhs is only used
					// Example:
					// Time is 10.000, so sampleTime is 9.900 
					// lhs.time is 9.910 rhs.time is 9.980 length is 0.070
					// t is 9.900 - 9.910 / 0.070 = 0.14. So it uses 14% of rhs, 86% of lhs
					if (length > 0.0001)
						t = (float)((interpolationTime - lhs.timestamp) / length);
					
					// if t=0 => lhs is used directly
					transform.localPosition = Vector3.Lerp(lhs.pos, rhs.pos, t);
					transform.localRotation = Quaternion.Slerp(lhs.rot, rhs.rot, t);
					return;
				}
			}
		}
		// Use extrapolation
		else
		{
			State latest = proxyStates[0];
			
			float extrapolationLength = (float)(interpolationTime - latest.timestamp);
			// Don't extrapolation for more than 500 ms, you would need to do that carefully
			if (extrapolationLength < extrapolationLimit)
			{				
				transform.position = latest.pos + latest.vel * extrapolationLength;
				transform.rotation = latest.rot;
				character.SimpleMove(latest.vel);
			}
		}
	}

	
	[RPC]
	void GoodOwnerPos(uLink.NetworkMessageInfo info)
	{
		Move goodMove;
		goodMove.timestamp = info.timestamp;
		goodMove.deltaTime = 0;
		goodMove.vel = Vector3.zero;
		
		int index = ownerMoves.BinarySearch(goodMove);
		if (index < 0) index = ~index;
		
		ownerMoves.RemoveRange(0, index);
	}
	
	[RPC]
	void AdjustOwnerPos(Vector3 pos, uLink.NetworkMessageInfo info)
	{
		GoodOwnerPos(info);
		
		transform.position = pos;
		
		foreach (Move move in ownerMoves)
		{
			character.Move(move.vel * move.deltaTime);
		}
	}
}
