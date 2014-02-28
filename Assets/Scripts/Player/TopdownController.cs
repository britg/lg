using UnityEngine; 
using System.Collections;

public class TopdownController : LGMonoBehaviour {

	public bool movedThisFrame = false;
	
	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;

	void Start () {
		AssignPlayer();
		controller = GetComponent<CharacterController>();
	}

	void Update() {
		if (player.isOwner) {
			DetectInput();
			RotatePlayer();
		}
	}

	void DetectInput () {
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		if (moveDirection.sqrMagnitude > 0f && player.hasEnoughFuel(Time.deltaTime)) {
			Move(moveDirection);
			movedThisFrame = true;
		} else {
			movedThisFrame = false;
		}
	}

	void Move (Vector3 dir) {
		float speed = player.stat(LG.s_speed);
		Debug.Log ("Speed is " + speed);
		Vector3 moveV = dir * speed * Time.deltaTime;
		controller.Move(moveV);
//		player.UseFuel(Time.deltaTime);
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