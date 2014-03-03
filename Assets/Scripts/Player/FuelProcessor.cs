using UnityEngine;
using System.Collections;
using uLink;

public class FuelProcessor : LGMonoBehaviour {

	void Start () {
	}

	public bool HasEnoughFuel (float time) {
		float currentFuel = playerProcessor.stats.Fuel;
		return (currentFuel - AmountForTime(time) > 0);
	}

	public void UseFuel (float time) {
		float amount = AmountForTime(time);
		playerProcessor.stats.Remove(LG.s_fuel, amount);
	}

	public float AmountForTime (float time) {
		return (time * playerProcessor.stats.FuelBurn);
	}

}
