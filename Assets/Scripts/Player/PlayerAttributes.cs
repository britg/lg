using UnityEngine;
using System;
using System.Collections;

public class PlayerAttributes : LGMonoBehaviour {

	public bool isOwner = false;

	[Serializable]
	public class ShipAttributes {

		public string name = "Starter Ship";

		public int shields = 0;
		public int hull = 100;
		public int fuel = 100;
		public int fuelBurn = 2;

	}

	[Serializable]
	public class WeaponAttributes {

		public int ammo = 100;
		public int ammoBurn = 1;
		public float cooldown = 0.2f;
		public Vector3 aim = Vector3.zero;
		public float velocity = 500f;
		public float life = 2f;

		public bool hasEnoughAmmo () {
			return hasEnoughAmmo(ammoBurn);
		}

		public bool hasEnoughAmmo (int request) {
			return (ammo - request) >= 0;
		}

		public void UseAmmo () {
			UseAmmo (ammoBurn);
		}

		public void UseAmmo (int amount) {
			ammo -= amount;
		}

	}

	public ShipAttributes shipAttributes = new ShipAttributes();
	public WeaponAttributes weaponAttributes = new WeaponAttributes();

	void Start () {
		if (isOwner) {
			AnnounceLoaded();
		}
	}

	void AnnounceLoaded() {
		Hashtable notificationData = new Hashtable();
		notificationData[LG.n_playerKey] = gameObject;
		NotificationCenter.PostNotification(this, LG.n_playerLoaded, notificationData);
	}

	[RPC]
	void SyncAmmo (int ammo) {
		weaponAttributes.ammo = ammo;
	}

}
