using UnityEngine;
using System.Collections;

public class Weapon : LGMonoBehaviour {

	GameObject _projectileLauncher;
	GameObject projectileLauncher {
		get {
			if (_projectileLauncher == null) {
				_projectileLauncher = transform.Find("ProjectileLauncher").gameObject;
			}
			return _projectileLauncher;
		}
	}

	void Fire (Transform target) {
		projectileLauncher.SendMessage("Fire", target);
	}
}
