using UnityEngine;
using System.Collections;

public class FuelController : LGMonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool HasEnoughFuel (float time) {
		float currentFuel = player.stats.Fuel;
		return (currentFuel - AmountForTime(time) > 0);
	}

	public float AmountForTime (float time) {
		return (time * player.stats.FuelBurn);
	}

}
