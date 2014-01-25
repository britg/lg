using UnityEngine; 
using System.Collections;

public class TopdownController : uLink.MonoBehaviour {
	
	//Variables
	public float turnspeed=180f;
	public float speed = 6.0F;
	private Vector3 moveDirection = Vector3.zero;

	void Start () {
	}

	void Update() {
		MovePlayer();
		//RotatePlayer();
	}

	void MovePlayer () {
		CharacterController controller = GetComponent<CharacterController>();
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		moveDirection = moveDirection * speed;
		controller.Move(moveDirection * Time.deltaTime);
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