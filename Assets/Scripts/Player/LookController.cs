using UnityEngine;
using System.Collections;
using Vectrosity;

public class LookController : LGMonoBehaviour {

	public Color cursorColor = Color.gray;
	public Vector3 worldLookPoint = Vector3.zero;
	public Vector3 lookDirection {
		get {
			return (worldLookPoint - transform.position).normalized;
		}
	}

	Transform lookAnchor;
	VectorLine pointer;
	GameObject pointerObj;
	Vector2 lastMousePos;

	// Use this for initialization
	void Start () {
		lookAnchor = GameObject.Find("LookAnchor").transform;
		lastMousePos = Vector2.zero;
		CreatePointer();
	}
	
	// Update is called once per frame
	void Update () {
		FollowMouse();
		FollowThumbstick();
	}

	void CreatePointer () {
		pointer = VectorLine.SetLine3D(cursorColor, lookAnchor.position, Vector3.up*1000);
		pointer.Draw();
		pointerObj = GameObject.Find("Vector SetLine3D");
		pointerObj.transform.parent = lookAnchor;
	}

	void FollowMouse () {
		Vector2 mousePos = Input.mousePosition;
		if (lastMousePos.Equals(mousePos)) {
			return;
		}
		lastMousePos = mousePos;
		Ray ray = Camera.main.ScreenPointToRay(mousePos);
		float dist;
		LG.plane.Raycast(ray, out dist);
		Vector3 approx = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, dist));
		approx.z = 0;
		worldLookPoint = approx;
		UpdateLook();
	}

	void FollowThumbstick () {
//		Vector2 lookDirection = new Vector2(Input.GetAxis("Look X"), -Input.GetAxis("Look Y")).normalized * 1;
//		if (lookDirection.sqrMagnitude > 0f) {
//			Vector2 screenLookPoint = playerScreenPos + lookDirection;
//			UpdateLook (screenLookPoint);
//		}
	}

	void UpdateLook () {
		pointer.points3[1] = lookDirection * 1000000;
	}

	void uLink_OnDisconnectedFromServer (uLink.NetworkDisconnection mode) {
		VectorLine.Destroy(ref pointer);
	}
}
