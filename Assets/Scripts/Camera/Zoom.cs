using UnityEngine;
using System.Collections;

public class Zoom : MonoBehaviour {

	public float multiplier = 2f;
	public float max = -300f;
	public float min = -800f;

	void Update () {
		DetectInput();
	}

	void DetectInput () {
		float wheelInput = Input.GetAxis ("Mouse ScrollWheel");
		if(!wheelInput.Equals(0f)) {
			Vector3 pos = transform.position;
			pos.z = Mathf.Clamp(pos.z + wheelInput, min, max);
			transform.position = pos;
		}
	}
}
