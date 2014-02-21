using UnityEngine;
using System.Collections;

public class ExtractorController : LGMonoBehaviour {

	public Transform extractorTurnPoint;
	public ParticleSystem extractor;

	TopdownLook topdownLook;

	bool isExtracting = false;
	float extractRPCRate = 0.1f;

	void Start () {
		AssignPlayerAttributes();
		topdownLook = GetComponent<TopdownLook>();
	}

	// Update is called once per frame
	void Update () {
		DetectInput();
		TurnExtractor();
	}

	void DetectInput () {
		if (Input.GetMouseButtonDown(1)) {
			StartExtracting();
		} else if (Input.GetMouseButtonUp(1)) {
			StopExtracting();
		}
	}

	void StartExtracting () {
		isExtracting = true;
		extractor.Play();
		InvokeRepeating("RemoteExtract", extractRPCRate, extractRPCRate);
	}

	void RemoteExtract () {
		networkView.UnreliableRPC("SignalExtract", uLink.RPCMode.Server, topdownLook.worldLookPoint.normalized);
	}

	void StopExtracting () {
		isExtracting = false;
		extractor.Stop();
		networkView.UnreliableRPC("StopExtracting", uLink.RPCMode.Server);
		CancelInvoke();
	}

	void TurnExtractor () {
		Vector3 angles = extractorTurnPoint.eulerAngles;
		float playerAngle = transform.eulerAngles.z;
		float lookAngle = Vector3.Angle(Vector3.up, topdownLook.worldLookPoint);
		angles.z = lookAngle;
		if (topdownLook.worldLookPoint.x > 0) {
			angles.z = -lookAngle;
		}
		extractorTurnPoint.eulerAngles = angles;
	}
}
