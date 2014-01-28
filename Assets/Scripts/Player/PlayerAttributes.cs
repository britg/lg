using UnityEngine;
using System;
using System.Collections;

public class PlayerAttributes : LGMonoBehaviour {

	[Serializable]
	public class ShipAttributes {

		public string name = "Starter Ship";

		public int shields = 0;
		public int hull = 100;
		public int fuel = 100;

	}

	[Serializable]
	public class WeaponAttributes {

		public float cooldown = 0.2f;
	}

	public ShipAttributes shipAttributes = new ShipAttributes();
	public WeaponAttributes weaponAttributes = new WeaponAttributes();

}
