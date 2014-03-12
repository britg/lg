// (c)2011 MuchDifferent. All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;
using uLink;

[RequireComponent(typeof(uLinkNetworkView))]
public class MoveController : LGMonoBehaviour
{


	private CharacterController character;
	private MoveSerializer moveSerializer;

	bool movedThisFrame = false;
	
	private Vector3 moveDirection = Vector3.zero;


	void Awake() {
		character = GetComponent<CharacterController>();
	}
	
	void Start () {
		moveSerializer = GetComponent<MoveSerializer>();
		StartCameraFollow();
	}

	void StartCameraFollow () {
//		TopFollow topFollow = Camera.main.GetComponent<TopFollow>();
//		topFollow.player = gameObject;
//		GameObject follower = new GameObject("PlayerFollower");
//		follower.transform.parent = transform;
//		follower.transform.localPosition = Vector3.zero;

//		Camera.main.transform.parent = ;
	}
	
	void Update() {
		if (player.isOwner) {
			DetectInput();
			RotatePlayer();
		}
	}
	
	void DetectInput () {
		Vector3 raw = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		moveDirection = Vector3.ClampMagnitude(raw, 1f);
		if (moveDirection.sqrMagnitude > 0f && player.fuelController.HasEnoughFuel(Time.deltaTime)) {
			Move(moveDirection);
			movedThisFrame = true;
		} else {
			movedThisFrame = false;
		}
	}
	
	void Move (Vector3 dir) {
		float speed = player.stat(Stat.speed);
		Vector3 moveV = dir * speed * Time.deltaTime;
		character.Move(moveV);
	}
	
	void RotatePlayer () {
		if (moveDirection.magnitude == 0f) {
			return;
		}
		
		float angle = -Vector3.Angle(Vector3.up, moveDirection);
		
		if (moveDirection.x < 0f) {
			angle *= -1f;
		}
		
		transform.eulerAngles = new Vector3(0, 0, angle);
	}

	// We have a window of interpolationBackTime where we basically play 
	// By having interpolationBackTime the average ping, you will usually use interpolation.
	// And only if no more data arrives we will use extra polation

	
	void LateUpdate()
	{
		if (!uLink.Network.isAuthoritativeServer || uLink.Network.isServerOrCellServer || !networkView.isMine)
		{
			return;
		}
		
		if (movedThisFrame) {
			MoveSerializer.Move move;
			move.timestamp = uLink.Network.time;
			move.deltaTime = (moveSerializer.ownerMoves.Count > 0) ? (float)(move.timestamp - moveSerializer.ownerMoves[moveSerializer.ownerMoves.Count - 1].timestamp) : 0.0f;
			move.vel = character.velocity;

			moveSerializer.ownerMoves.Add(move);
			networkView.UnreliableRPC("ServerMove", uLink.NetworkPlayer.server, transform.position, move.vel, transform.rotation);
		} else {
			networkView.UnreliableRPC("ServerIdleTime", uLink.NetworkPlayer.server);
		}
	}

}
