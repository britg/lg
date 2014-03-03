using UnityEngine;
using System.Collections;

public class DamageProcessor : LGMonoBehaviour {

	void Awake () {
		NotificationCenter.AddObserver(this, LG.n_playerHit);
	}

	void OnPlayerHit (Notification n) {
		Hashtable nData = n.data;

		GameObject shipMesh = (GameObject)nData["hit"];

		if (shipMesh == null)
			return;

		GameObject hit;
		try {
			hit = shipMesh.transform.parent.transform.parent.gameObject;
		} catch {
			return;
		}

		if (hit == gameObject) {

		}
	}

}
