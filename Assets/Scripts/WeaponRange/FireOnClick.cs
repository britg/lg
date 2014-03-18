using UnityEngine;
using System.Collections;

public class FireOnClick : MonoBehaviour {

	public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		DetectInput();
	}

	void DetectInput () {
		if (Input.GetMouseButtonDown(0)) {
			SendFire();
		}
	}

	void SendFire () {
		foreach (Transform child in transform) {
			Weapon weapon = child.gameObject.GetComponent<Weapon>();
			if (weapon != null) {
				weapon.Fire(target);
			}
		}
	}
}

