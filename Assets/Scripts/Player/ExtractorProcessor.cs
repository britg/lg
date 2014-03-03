using UnityEngine;
using System.Collections;

public class ExtractorProcessor : LGMonoBehaviour {

	bool isExtracting = false;
	Vector3 direction = Vector3.zero;

	void Start () {
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
		float rate = player.stats.Get("extractor_rate").value;
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
		float extendLength = player.stats.Get("extractor_length").value;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, direction, out hit, extendLength)) {
//			Debug.Log ("Extractor hit something " + hit.collider.gameObject);
			hit.collider.gameObject.SendMessage("Extract", this, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void YieldResources (Resource[] resources) {
		networkView.UnreliableRPC("AddElementStores", uLink.RPCMode.Owner, resources);
	}
}
