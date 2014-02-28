using UnityEngine;
using System.Collections;

public class Asteroid : WorldObject {

	public int refillInterval = 300;

	void Awake () {
		Refill();
		InvokeRepeating ("Refill", refillInterval, refillInterval);
	}

	new void AssignAttributes (Hashtable attributes) {
		base.AssignAttributes(attributes);
	}

	void Extract (ExtractorProcessor processor) {

	}

	void Refill () {

	}

}
