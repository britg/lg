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
		target = player.transform;
		ZoomController zoom = Camera.main.GetComponent<ZoomController>();
		Camera.main.transform.parent = transform;
		Camera.main.transform.localPosition = zoom.offset;
	}

}
