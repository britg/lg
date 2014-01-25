using UnityEngine;
using System.Collections;
using Vectrosity;

public class PlayerMouseLook : MonoBehaviour {

	private Vector2 playerScreenPos;
	private VectorLine pointer;
	private Vector2 lastMousePos;


	// Use this for initialization
	void Start () {
		AssignPlayerPos();
		CreatePointer();
		lastMousePos = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {
		PointAtMouse();
		FollowThumbstick();
	}

	void AssignPlayerPos () {
		float x = Screen.width / 2f;
		float y = Screen.height / 2f;
		playerScreenPos = new Vector2(x, y);
	}

	void CreatePointer () {
		pointer = VectorLine.SetLine(Color.green, playerScreenPos, playerScreenPos);
	}

	void PointAtMouse () {
		Vector2 mousePos = Input.mousePosition;

		if (lastMousePos.Equals(mousePos)) {
			return;
		}

		Vector2 towards = (mousePos - playerScreenPos) * 100;
		pointer.points2[1] = playerScreenPos + towards;
		pointer.Draw();

		lastMousePos = mousePos;
	}

	void FollowThumbstick () {
		Vector2 lookDirection = new Vector2(Input.GetAxis("Look X"), -Input.GetAxis("Look Y")).normalized * 1000;
		if (lookDirection.sqrMagnitude > 0f) {
			pointer.points2[1] = playerScreenPos + lookDirection;
			pointer.Draw();
		}
	}
}
