using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : LGMonoBehaviour {

	public bool isOwner = false;
	public string playerName = "Player";
	public int playerId;
	public StatCollection stats;

	[Serializable]
	public class ShipAttributes {

		public string name = "Starter Ship";

		public int 		shields = 0;
		public int 		hull = 100;
		public float 	fuel = 100f;
		public float 	fuelBurn = 2f;
		public float 	speed = 6f;

		public float 	extractorRate = 1f;
		public float	extractorLength = 100f;
		public float 	extractorAngle = 15f;
		public float 	extractorRadius = 22.5f;

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

		public int 		ammo = 100;
		public int 		ammoBurn = 1;
		public float 	cooldown = 0.2f;
		public Vector3 	aim = Vector3.zero;
		public float 	velocity = 500f;
		public float 	life = 2f;
		public float 	damage = 10f;

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
			AssignNotifier();
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

	public void SetStats (Hashtable serverStats) {
		stats.Set (serverStats);
	}

	public void SyncAttributes (string rawAttributes) {
		Debug.Log ("Syncing attribtues from server to client");
//		networkView.RPC ("AssignAttributes", uLink.RPCMode.All, rawAttributes);
	}

	[RPC]
	public void AssignAttributes (string rawAttributes) {
		Debug.Log ("Assigning attributes to this player " + rawAttributes);
		Hashtable attributes = MiniJSON.Json.Hashtable(rawAttributes);

		IDictionary props = (IDictionary) attributes["properties"];

		// position
//		Vector3 pos = transform.position;
//		Vector3 eulerAngles = transform.eulerAngles;
//		float.TryParse(props["x"].ToString(), out pos.x);
//		float.TryParse(props["y"].ToString(), out pos.y);
//		float.TryParse(props["z"].ToString(), out eulerAngles.z);
//		pos.z = 0f;
//		transform.position = pos;
//		transform.eulerAngles = eulerAngles;

		int.TryParse(props["shields"].ToString(), out shipAttributes.shields);
		int.TryParse(props["hull"].ToString(), out shipAttributes.hull);
		float.TryParse(props["fuel"].ToString(), out shipAttributes.fuel);
		int.TryParse(props["ammo"].ToString(), out weaponAttributes.ammo);

		if (isOwner) {
			AnnounceLoaded();
		}
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

	[RPC]
	void AddResources (Resource[] resources) {
//		notifier.Notify (gameObject, toAdd.ToFloatingText());
	}

	public void RequestRespawn () {
		networkView.RPC ("Respawn", uLink.RPCMode.Server);
	}

}
