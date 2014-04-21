// (c)2011 MuchDifferent. All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;

public class ExploreController : ControllerBehaviour {

	public float speed = 100f;
	
	CharacterController character;
	Vector3 moveDirection = Vector3.zero;
	Transform referenceFrame;

	
	void Awake() {
		character = GetComponent<CharacterController>();
	}
	
	void Start () {
		referenceFrame = transform;
		NotificationCenter.PostNotification(this, LG.n_playerStatsLoaded);
	}
	
	protected override void DetectInput () {
		Vector3 raw = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		Quaternion offset = Quaternion.Euler(referenceFrame.eulerAngles);
		moveDirection = offset * Vector3.ClampMagnitude(raw, 1f);
		if (moveDirection.sqrMagnitude > 0) {
			Move(moveDirection);
		} 
	}
	
	void Move (Vector3 dir) {
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

}
