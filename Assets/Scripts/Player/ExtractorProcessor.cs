using UnityEngine;
using System.Collections;

public class ExtractorProcessor : LGMonoBehaviour {

	bool isExtracting = false;
	Vector3 direction = Vector3.zero;

	void Start () {
		AssignPlayerAttributes();
	}

	[RPC]
	void SignalExtract (Vector3 _dir) {
		direction = _dir;
		if (!isExtracting) {
			StartExtracting();
		}
	}

	void StartExtracting () {
		isExtracting = true;
		float rate = playerAttributes.shipAttributes.extractorRate;
		InvokeRepeating("Extract", rate, rate);
	}

	[RPC]
	void StopExtracting () {
		isExtracting = false;
		CancelInvoke();
	}

	void Extract () {
		DetectHit();
	}

	void DetectHit () {
		float extendLength = playerAttributes.shipAttributes.extractorLength;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, direction, out hit, extendLength)) {
			Debug.Log ("Extractor hit something " + hit.collider.gameObject);
			hit.collider.gameObject.SendMessage("Extract", this, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void YieldElements (ElementYield e) {
		Debug.Log ("Elements yielded " + e);
		playerAttributes.elementStores.Add (e);
		Debug.Log ("New elementStores is " + playerAttributes.elementStores);
	}
}
