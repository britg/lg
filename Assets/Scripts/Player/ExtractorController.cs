using UnityEngine;
using System.Collections;

public class ExtractorController : LGMonoBehaviour {

	public Transform extractorTurnPoint;
	public ParticleSystem extractor;

	LookController topdownLook;

	bool isExtracting = false;
	float extractRPCRate = 0.1f;

	void Start () {
		topdownLook = GetComponent<LookController>();
	}

	// Update is called once per frame
	void Update () {
		DetectInput();
		TurnExtractor();
		if(isExtracting) {
//			DebugExtractor();
		}
	}

	void DetectInput () {
		if (Input.GetMouseButtonDown(1)) {
			StartExtracting();
		} else if (Input.GetMouseButtonUp(1)) {
			StopExtracting();
		}
	}

	void StartExtracting () {
		if (isExtracting) {
			return;
		}
		isExtracting = true;
		extractor.Play();
		InvokeRepeating("RemoteExtract", extractRPCRate, extractRPCRate);

	}

	void RemoteExtract () {
		networkView.UnreliableRPC("SignalExtract", uLink.RPCMode.Server, topdownLook.worldLookPoint);
	}

	void StopExtracting () {
		isExtracting = false;
		extractor.Stop();
		networkView.UnreliableRPC("StopExtracting", uLink.RPCMode.Server);
		CancelInvoke();
	}

	void TurnExtractor () {
		Vector3 angles = extractorTurnPoint.eulerAngles;
		float lookAngle = Vector3.Angle(Vector3.up, topdownLook.worldLookPoint);
		angles.z = lookAngle;
		if (topdownLook.worldLookPoint.x > 0) {
			angles.z = -lookAngle;
		}
		extractorTurnPoint.eulerAngles = angles;
	}

	void DebugExtractor () {
		float extendLength = player.stat(Stat.extractorLength);
		Vector3 from = transform.position;
		Vector3 to = topdownLook.worldLookPoint;
		Vector3 dir = (to - from).normalized*extendLength;
		Debug.Log ("Drawing ray from " + from + " to " + (from + dir));
		Debug.DrawRay(from, dir, Color.red);
	}
}
