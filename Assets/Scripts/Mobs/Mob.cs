using UnityEngine;
using System;
using System.Collections;

public class Mob : WorldObject {

	public bool alive = true;

	void Start () {

	}

	public override void TakeDamage (float amount) {
		Debug.Log ("Mob take damage " + amount);
		float currentShields = stats.Get(Stat.shields).value;
		float currentHull = stats.Get(Stat.hull).value;
		float diff = 0f;

		if (currentShields > amount) {
			stats.Remove(Stat.shields, amount);
		}

		if (currentShields < amount) {
			diff = amount - currentShields;
			stats.Set(Stat.shields, 0f);
			stats.Remove(Stat.hull, diff);

			if (currentHull < diff) {
				Die();
			}
		}

		networkView.UnreliableRPC(DamageDisplay.Client_Display, uLink.RPCMode.Others, amount);
	}

	public void Die () {

	}

}
