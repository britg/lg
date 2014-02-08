using UnityEngine;
using System;
using System.Collections;

public class PlayerAttributes : LGMonoBehaviour {

	public bool isOwner = false;
	public string playerName = "Player";
	public int playerId;

	[Serializable]
	public class ShipAttributes {

		public string name = "Starter Ship";

		public int shields = 0;
		public int hull = 100;
		public float fuel = 100f;
		public float fuelBurn = 2f;
		public float speed = 6f;

		public bool hasEnoughFuel (float deltaTime) {
			return (fuel - fuelBurn*deltaTime) >= 0f;
		}

		public float UseFuel (float deltaTime) {
			fuel -= deltaTime * fuelBurn;
			return fuel;
		}

	}

	[Serializable]
	public class WeaponAttributes {

		public int ammo = 100;
		public int ammoBurn = 1;
		public float cooldown = 0.2f;
		public Vector3 aim = Vector3.zero;
		public float velocity = 500f;
		public float life = 2f;
		public float damage = 10f;

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

	void uLink_OnNetworkInstantiate (uLink.NetworkMessageInfo info) {
		info.networkView.initialData.TryRead<string>(out playerName);
		info.networkView.initialData.TryRead<int>(out playerId);
		TextMesh nameLabel = transform.Find("Nametag").GetComponent<TextMesh>();
		nameLabel.text = playerName;
	}
	
	void AnnounceLoaded() {
		Hashtable notificationData = new Hashtable();
		notificationData[LG.n_playerKey] = gameObject;
		NotificationCenter.PostNotification(this, LG.n_playerLoaded, notificationData);
	}

	[RPC]
	public void SyncAttributes (Hashtable attributes) {

	}

	[RPC]
	public void SyncStartPostition (Vector3 pos) {
		transform.position = pos;
	}

	[RPC]
	void SyncAmmo (int ammo) {
		weaponAttributes.ammo = ammo;
	}

	[RPC]
	void SyncFuel (float fuel) {
		shipAttributes.fuel = fuel;
	}

	[RPC]
	void SyncDamage (float damage) {
		Debug.Log ("Taking damage ! " + damage);
		shipAttributes.hull -= (int)damage;

		if (shipAttributes.hull <= 0f) {
			Destroy (gameObject);
		}
	}

}
