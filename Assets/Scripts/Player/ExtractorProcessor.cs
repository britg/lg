using UnityEngine;
using System.Collections;

public class ExtractorProcessor : LGMonoBehaviour {

	bool isExtracting = false;
	Vector3 direction = Vector3.zero;

	void Start () {
	}

	[RPC]
	void SignalExtract (Vector3 worldLookPoint) {
		direction = (worldLookPoint - transform.position).normalized;
		if (!isExtracting) {
			StartExtracting();
		}
	}

	void StartExtracting () {
		isExtracting = true;
		float rate = playerProcessor.stat(Stat.extractorRate);
		InvokeRepeating("Extract", rate, rate);
	}

	[RPC]
	void StopExtracting () {
		isExtracting = false;
		CancelInvoke();
	}

	void Extract () {
		Debug.Log ("Extracting " + direction);
		DetectHit();
	}

	void DetectHit () {
		float extendLength = playerProcessor.stat(Stat.extractorLength);
		RaycastHit hit;
		if (Physics.Raycast(transform.position, direction, out hit, extendLength)) {
//			Debug.Log ("Extractor hit something " + hit.collider.gameObject);
			hit.collider.gameObject.SendMessage("Extract", this, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void YieldResources (Resource[] resources) {
		Debug.Log ("Yielded resources " + resources[0]);
		foreach (Resource r in resources) {
			playerProcessor.resources.Add (r);
		}
	}
}
