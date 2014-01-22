using UnityEngine;
using System.Collections;
using Vectrosity;

public class PlayerMouseLook : MonoBehaviour {

	private Vector2 playerScreenPos;
	private VectorLine pointer;

	// Use this for initialization
	void Start () {
		AssignPlayerPos();
		CreatePointer();
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

	void PointAtMouse () {
		Vector2 mousePosition = Input.mousePosition;
		Vector2 towards = (mousePosition - playerScreenPos) * 100;
		pointer.points2[1] = playerScreenPos + towards;
		pointer.Draw();
	}
}
