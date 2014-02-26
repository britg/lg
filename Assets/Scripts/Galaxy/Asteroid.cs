using UnityEngine;
using System.Collections;

public class Asteroid : WorldObject {

	public ElementYield elementStore;
	public int refillInterval = 300;

	void Awake () {
		elementStore = new ElementYield();
		Refill();
		InvokeRepeating ("Refill", refillInterval, refillInterval);
	}

	new void AssignAttributes (Hashtable attributes) {
		base.AssignAttributes(attributes);
	}

	void Extract (ExtractorProcessor processor) {
		ElementYield e = new ElementYield();
		int amount = (int)Random.Range(0f, 5f);
		if (elementStore.hydrogen >= amount) {
			e.hydrogen = amount;
		} else {
			e.hydrogen = elementStore.hydrogen;
		}

		amount = (int)Random.Range(0f, 5f);
		if (elementStore.carbon >= amount) {
			e.carbon = amount;
		} else {
			e.carbon = elementStore.carbon;
		}

		amount = (int)Random.Range(0f, 5f);
		if (elementStore.trace >= amount) {
			e.trace = amount;
		} else {
			e.trace = elementStore.trace;
		}

		elementStore.Remove(e);
		processor.YieldElements(e);
	}

	void Refill () {
		elementStore.oxygen = (int)Random.Range (0f, 2f);
		elementStore.hydrogen = (int)Random.Range (0f, 10f);
		elementStore.carbon = (int)Random.Range (0f, 10f);
		elementStore.trace = (int)Random.Range (5f, 20f);
	}

}
