using UnityEngine;
using System.Collections;
using Vectrosity;

public class TopdownLook : LGMonoBehaviour {

	public Vector3 worldLookPoint;

	private Vector2 playerScreenPos;
	private VectorLine pointer;
	private Vector2 lastMousePos;

	// Use this for initialization
	void Start () {
		AssignPlayerAttributes();
		if (playerAttributes.isOwner) {
			AssignPlayerPos();
			CreatePointer();
			lastMousePos = Vector2.zero;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (playerAttributes.isOwner) {
			FollowMouse();
			FollowThumbstick();
		}
	}

	void AssignPlayerPos () {
		float x = Screen.width / 2f;
		float y = Screen.height / 2f;
		playerScreenPos = new Vector2(x, y);
	}

	void CreatePointer () {
		pointer = VectorLine.SetLine(Color.green, playerScreenPos, playerScreenPos);
	}

	void FollowMouse () {
		Vector2 mousePos = Input.mousePosition;

		if (lastMousePos.Equals(mousePos)) {
			return;
		}

		Vector2 screenLookPoint = playerScreenPos + (mousePos - playerScreenPos) * 100;
		UpdateLook (screenLookPoint);

		lastMousePos = mousePos;
	}

	void FollowThumbstick () {
		Vector2 lookDirection = new Vector2(Input.GetAxis("Look X"), -Input.GetAxis("Look Y")).normalized * 1000;
		if (lookDirection.sqrMagnitude > 0f) {
			Vector2 screenLookPoint = playerScreenPos + lookDirection;
			UpdateLook (screenLookPoint);
		}
	}

	void UpdateLook (Vector2 screenLookPoint) {
		worldLookPoint = transform.position + Camera.main.ScreenToWorldPoint(new Vector3(screenLookPoint.x, screenLookPoint.y, -Camera.main.transform.position.z));
		pointer.points2[1] = screenLookPoint;
		pointer.Draw();
	}

	void uLink_OnDisconnectedFromServer (uLink.NetworkDisconnection mode) {
		VectorLine.Destroy(ref pointer);
	}
}
