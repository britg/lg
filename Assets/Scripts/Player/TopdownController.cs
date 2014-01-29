using UnityEngine; 
using System.Collections;

public class TopdownController : LGMonoBehaviour {
	
	//Variables
	public float turnspeed=180f;
	public float speed = 6.0F;

	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;

	void Start () {
		controller = GetComponent<CharacterController>();
	}

	void Update() {
		DetectInput();
		RotatePlayer();
	}

	void DetectInput () {
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		if (moveDirection.sqrMagnitude > 0f) {
			Move(moveDirection);
		}
	}

	void Move (Vector3 dir) {
		Vector3 moveV = dir * speed * Time.deltaTime;
		controller.Move(moveV);
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