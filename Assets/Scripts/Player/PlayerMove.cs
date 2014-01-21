using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	public float moveSpeed = 10.0f;
	private Vector3 delta;

	void Start () {
	}
	
	void Update () {
		if (networkView.isMine) {
			MovePlayer();
			RotatePlayer();
		}
	}

	void MovePlayer () {
		float deltaX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
		float deltaY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
		delta = new Vector3(deltaX, deltaY, 0f);

		transform.Translate(delta, Space.World);
	}

	void RotatePlayer () {
		if (delta.magnitude == 0f) {
			return;
		}

		float angle = -Vector3.Angle(Vector3.up, delta);

		if (delta.x < 0f) {
			angle *= -1f;
		}

		transform.eulerAngles = new Vector3(0, 0, angle);
	}
}
