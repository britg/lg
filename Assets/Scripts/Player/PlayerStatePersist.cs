using UnityEngine;
using System.Collections;

public class PlayerStatePersist : PersistenceRequest {

	public float repeatRate = 1f;
	bool shouldSync = true;
	bool lastSyncReturned = false;

	void Start () {
		AssignPlayerAttributes();
		StartSync();
	}

	void StartSync () {
		lastSyncReturned = true;
		InvokeRepeating("Sync", repeatRate, repeatRate);
	}

	void Sync () {
		if (!lastSyncReturned) {
//			return;
		}

		if (!shouldSync) {
			return;
		}

		lastSyncReturned = false;

		Vector3 pos = transform.position;
		Quaternion rot = transform.rotation;
		WWWForm formData = new WWWForm();

		// Position
		formData.AddField("player[x]", pos.x.ToString());
		formData.AddField("player[y]", pos.y.ToString());
		formData.AddField("player[z]", rot.eulerAngles.z.ToString());

		// Ship status
		formData.AddField("player[shields]", playerAttributes.shipAttributes.shields.ToString());
		formData.AddField("player[hull]", playerAttributes.shipAttributes.hull.ToString());
		formData.AddField("player[fuel]", playerAttributes.shipAttributes.fuel.ToString());
		formData.AddField("player[fuel_burn]", playerAttributes.shipAttributes.fuelBurn.ToString());
		formData.AddField("player[speed]", playerAttributes.shipAttributes.speed.ToString());

		// Weapon attributes
		formData.AddField("player[ammo]", playerAttributes.weaponAttributes.ammo.ToString());
		formData.AddField("player[ammo_burn]", playerAttributes.weaponAttributes.ammoBurn.ToString());
		formData.AddField("player[cooldown]", playerAttributes.weaponAttributes.cooldown.ToString());
		formData.AddField("player[ammo_velocity]", playerAttributes.weaponAttributes.velocity.ToString());
		formData.AddField("player[ammo_duration]", playerAttributes.weaponAttributes.life.ToString());
		formData.AddField("player[ammo_damage]", playerAttributes.weaponAttributes.damage.ToString());

		// Element Stores
		formData.AddField("player[oxygen]", playerAttributes.elementStores.oxygen.ToString());
		formData.AddField("player[hydrogen]", playerAttributes.elementStores.hydrogen.ToString());
		formData.AddField("player[nitrogen]", playerAttributes.elementStores.nitrogen.ToString());
		formData.AddField("player[carbon]", playerAttributes.elementStores.carbon.ToString());
		formData.AddField("player[trace]", playerAttributes.elementStores.trace.ToString());

		Put ("/players/" + playerAttributes.playerId, formData, SyncSuccess);
	}

	void SyncSuccess (Hashtable response, object receiver) {
		lastSyncReturned = true;
	}

	[RPC]
	void Respawn () {
		shouldSync = false;
		Post ("/players/" + playerAttributes.playerId + "/respawn", new WWWForm(), RespawnSuccess);

	}
	
	void RespawnSuccess (Hashtable response, object receiver) {
		Debug.Log ("Respawn success ");
		playerAttributes.SyncAttributes((string)response["raw"]);
	}

/* :shields, :hull, :fuel, :fuel_burn, :speed,

                     :ammo, :ammo_burn, :cooldown,
                     :ammo_velocity, :ammo_duration, :ammo_damage] */

}
