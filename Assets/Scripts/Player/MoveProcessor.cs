using UnityEngine;
using System.Collections;

public class MoveProcessor : LGMonoBehaviour {
	
	double serverLastTimestamp = 0;
	
	public float sqrMaxServerError = 625.0f;
	public float sqrMaxServerSpeed = 100000.0f;
	
	private CharacterController character;

	void Start () {
		character = GetComponent<CharacterController>();
	}
	
	[RPC] // Called on server
	void ServerIdleTime (uLink.NetworkMessageInfo info) {
		serverLastTimestamp = info.timestamp;
	}
	
	[RPC] // Called on Server
	void ServerMove(Vector3 ownerPos, Vector3 vel, Quaternion rot, uLink.NetworkMessageInfo info)
	{
		if (info.timestamp <= serverLastTimestamp)
		{
			return;
		}
		
		transform.rotation = rot;
		
		if (vel.sqrMagnitude > sqrMaxServerSpeed)
		{
			vel.x = vel.y = Mathf.Sqrt(sqrMaxServerSpeed) / 3.0f;
		}
		
		float deltaTime = (float)(info.timestamp - serverLastTimestamp);
		Vector3 deltaPos = vel * deltaTime;
		deltaPos.z = 0;
		
		character.Move(deltaPos);
		
		serverLastTimestamp = info.timestamp;
		
		Vector3 serverPos = transform.position;
		Vector3 diff = serverPos - ownerPos;
		
		
		//		PlayerRequest playerRequest = (new GameObject()).AddComponent<PlayerRequest>();
		//		playerRequest.SetPlayerPosition(playerAttributes, serverPos);
		
		
		if (Vector3.SqrMagnitude(diff) > sqrMaxServerError)
		{
			networkView.UnreliableRPC("AdjustOwnerPos", uLink.RPCMode.Owner, serverPos);
		}
		else
		{
			networkView.UnreliableRPC("GoodOwnerPos", uLink.RPCMode.Owner);
		}
	}
}
