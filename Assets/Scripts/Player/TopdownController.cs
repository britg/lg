﻿using UnityEngine; 
using System.Collections;

public class TopdownController : LGMonoBehaviour {

	public bool movedThisFrame = false;
	
	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;

	void Start () {
		AssignPlayerAttributes();
		controller = GetComponent<CharacterController>();
	}

	void Update() {
		if (playerAttributes.isOwner) {
			DetectInput();
			RotatePlayer();
		}
	}

	void DetectInput () {
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		if (moveDirection.sqrMagnitude > 0f && playerAttributes.shipAttributes.hasEnoughFuel(Time.deltaTime)) {
			Move(moveDirection);
			movedThisFrame = true;
		} else {
			movedThisFrame = false;
		}
	}

	void Move (Vector3 dir) {
		float speed = playerAttributes.shipAttributes.speed;
		Vector3 moveV = dir * speed * Time.deltaTime;
		controller.Move(moveV);
		playerAttributes.shipAttributes.UseFuel(Time.deltaTime);
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