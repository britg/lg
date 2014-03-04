using UnityEngine; 
using System.Collections;

public class TopdownController : LGMonoBehaviour {

	public bool movedThisFrame = false;
	
	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;

	void Start () {
		controller = GetComponent<CharacterController>();
		StartCameraFollow();
	}

	void StartCameraFollow () {
		TopFollow topFollow = Camera.main.GetComponent<TopFollow>();
		topFollow.player = gameObject;
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