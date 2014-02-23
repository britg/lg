using UnityEngine;
using System.Collections;

public class Asteroid : WorldObject {

	public ElementYield elementStore;

	void Awake () {
		transform.parent = GameObject.Find ("WorldObjects").transform;
	}

	new void AssignAttributes (string rawAttributes) {
		base.AssignAttributes(rawAttributes);
	}

	void Extract (ExtractorProcessor processor) {
		ElementYield e = new ElementYield();
		processor.YieldElements(e);
	}

}
