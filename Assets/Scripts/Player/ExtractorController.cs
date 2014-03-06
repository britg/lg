using UnityEngine;
using System.Collections;

public class ExtractorController : ControllerBehaviour {

	public Transform extractorTurnPoint;
	public ParticleSystem extractor;

	bool isExtracting = false;
	float extractRPCRate = 0.1f;

	void Start () {
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
		networkView.UnreliableRPC("SignalExtract", uLink.RPCMode.Server, currentLookDirection);
	}

	void StopExtracting () {
		isExtracting = false;
		extractor.Stop();
		networkView.UnreliableRPC("StopExtracting", uLink.RPCMode.Server);
		CancelInvoke();
	}

	void TurnExtractor () {
		Vector3 angles = extractorTurnPoint.eulerAngles;
		float lookAngle = Vector3.Angle(Vector3.up, currentLookDirection);
		angles.z = lookAngle;
		if (currentLookDirection.x > 0) {
			angles.z = -lookAngle;
		}
		extractorTurnPoint.eulerAngles = angles;
	}

	void DebugExtractor () {
	}
}
