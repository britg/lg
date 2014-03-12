using UnityEngine;
using System.Collections;

public class ZoomController : MonoBehaviour {

	public Vector3 angles = new Vector3(323.55f, 0f, 0f);
	public Vector3 offset = new Vector3(0f, -370f, -500);

	public float multiplier = 2f;

	float currentDistance;
	public float min = 100f;
	public float max = 1800f;
	public float bounceTime = 1f;
	public float bounceAmount = 100f;

	public Transform target;

	void Start () {
		// Cache the 'Camera Anchor' transform;
		NotificationCenter.AddObserver(this, LG.n_cameraAnchored);
	}

	void OnCameraAnchored () {
		target = transform.parent.transform;
		transform.localPosition = offset;
	}

	void Update () {
		if (target == null) {
			return;
		}

		DetectInput();

		if (TooClose()) {
			BounceOut();
		}

		if (TooFar()) {
			BounceIn();
		}

//		RetainOffset();

	}

	void DetectInput () {
		float wheelInput = Input.GetAxis ("Mouse ScrollWheel");
		Vector3 pos = transform.position;
		currentDistance = Vector3.Distance(pos, target.position);
		if(!wheelInput.Equals(0f) && !(TooClose() || TooFar())) {
			Vector3 newPos = Vector3.MoveTowards(pos, target.position, wheelInput*multiplier);
			transform.position = newPos;
		}
	}

	bool TooClose () {
		return currentDistance <= min;
	}

	bool TooFar () {
		return currentDistance >= max;
	}

	void BounceOut () {
		Vector3 outPos = Vector3.MoveTowards(transform.localPosition, Vector3.zero, -bounceAmount);
		iTween.MoveTo(gameObject, iTween.Hash("position", outPos, "time", bounceTime, "isLocal", true));
	}

	void BounceIn () {
		Vector3 inPos = Vector3.MoveTowards(transform.localPosition, Vector3.zero, bounceAmount);
		iTween.MoveTo(gameObject, iTween.Hash("position", inPos, "time", bounceTime, "isLocal", true));
	}

	void RetainOffset () {
		transform.localPosition = offset;
	}

}
