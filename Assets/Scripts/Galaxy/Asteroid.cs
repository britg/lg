using UnityEngine;
using System.Collections;

public class Asteroid : WorldObject {

	void Awake () {
		transform.parent = GameObject.Find ("WorldObjects").transform;
	}

	new void AssignAttributes (string rawAttributes) {
		base.AssignAttributes(rawAttributes);
	}

}
