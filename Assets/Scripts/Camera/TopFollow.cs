using UnityEngine;
using System.Collections;

public class TopFollow : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null) {
			FollowPlayer();
		}
	}

	void FollowPlayer () {
		Vector3 pos = transform.position;
		pos.x = player.transform.position.x;
		pos.y = player.transform.position.y;

		transform.position = pos;
	}
}
