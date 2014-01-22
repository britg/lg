using UnityEngine;
using System.Collections;
using Vectrosity;

public class PlayerMouseLook : MonoBehaviour {

	private Vector2 playerScreenPos;
	private Vector3 playerWorldPos;
	private Vector3 lastMouseWorldPos;
	private VectorLine pointer;

	// Use this for initialization
	void Start () {
		AssignPlayerPos();
		CreatePointer();
		AssignMousePos();
	}
	
	// Update is called once per frame
	void Update () {
		PointAtMouse();
	}

	void AssignPlayerPos () {
		float x = Screen.width / 2f;
		float y = Screen.height / 2f;
		playerScreenPos = new Vector2(x, y);
	}

	void CreatePointer () {
		pointer = VectorLine.SetLine(Color.green, playerScreenPos, playerScreenPos);
	}

	void AssignMousePos () {
		lastMouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	void PointAtMouse () {
		Vector2 mouseScreenPos = Input.mousePosition;
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
		Vector2 towards = (mouseScreenPos - playerScreenPos) * 100;

		pointer.points2[1] = playerScreenPos + towards;
		pointer.Draw();
	}
}
