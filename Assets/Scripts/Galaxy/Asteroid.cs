using UnityEngine;
using System.Collections;

public class Asteroid : WorldObject {

	public int refillInterval = 300;

	void Awake () {
		Refill();
		InvokeRepeating ("Refill", refillInterval, refillInterval);
	}

	void Extract (ExtractorProcessor processor) {

	}

	void Refill () {

	}

}
