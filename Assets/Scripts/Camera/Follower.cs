using UnityEngine;
using System.Collections;

public class Follower : LGMonoBehaviour {

	Transform target;

	// Use this for initialization
	void Start () {
		NotificationCenter.AddObserver(this, LG.n_playerStatsLoaded);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (target != null) {
			transform.position = target.position;
		}
	}

	void OnPlayerStatsLoaded () {
		AnchorCamera();
	}

	void AnchorCamera () {
		target = player.transform;
		Camera.main.transform.parent = transform;
		NotificationCenter.PostNotification(this, LG.n_cameraAnchored);
	}

}
