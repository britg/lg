using UnityEngine;
using System.Collections;

public class ZoomController : MonoBehaviour {

	public Vector3 angles = new Vector3(323.55f, 0f, 0f);
	public Vector3 offset = new Vector3(0f, -370f, -500);

	public float multiplier = 2f;
	public float max = -300f;
	public float min = -800f;

	public float zoom = 1f;

	void Update () {
		DetectInput();
	}

	void DetectInput () {
		float wheelInput = Input.GetAxis ("Mouse ScrollWheel");
		if(!wheelInput.Equals(0f)) {
//			Vector3 pos = transform.position;
			zoom += wheelInput;
//			pos.z = Mathf.Clamp(pos.z + wheelInput, min, max);
//			transform.position = pos;
		}
	}
}
