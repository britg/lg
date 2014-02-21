using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	void Awake () {
		transform.parent = GameObject.Find ("WorldObjects").transform;
	}

}
