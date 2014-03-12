using UnityEngine;
using System.Collections;

public class LookAnchor : LGMonoBehaviour {

	Transform anchor;

	// Use this for initialization
	void Start () {
		NotificationCenter.AddObserver(this, LG.n_playerStatsLoaded);
	}

	void OnPlayerStatsLoaded () {
		anchor = player.transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Update is called once per frame
	void LateUpdate () {
		if (anchor != null) {
			transform.position = anchor.position;
		}
	}
}
