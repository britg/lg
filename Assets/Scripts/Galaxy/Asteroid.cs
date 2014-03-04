using UnityEngine;
using System.Collections;

public class Asteroid : WorldObject {

	public int refillInterval = 300;

	void Awake () {
		Refill();
		InvokeRepeating ("Refill", refillInterval, refillInterval);
	}

	void Extract (ExtractorProcessor processor) {
		Resource[] extraction = new Resource[1];
		extraction[0] = new Resource(Resource.tier1, 1);
		processor.YieldResources(extraction);
	}

	void Refill () {

	}

}
