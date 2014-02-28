using UnityEngine;
using System.Collections;

public class PlayerStatePersist : PersistenceRequest {

	public float repeatRate = 1f;
	bool shouldSync = true;
	bool lastSyncReturned = false;

	void Start () {
		AssignPlayer();
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
		WWWForm formData = player.stats.toFormData();

		Put ("/players/" + player.playerId, formData, SyncSuccess);
	}

	void SyncSuccess (Hashtable response, object receiver) {
		lastSyncReturned = true;
	}

	[RPC]
	void Respawn () {
		shouldSync = false;
		Post ("/players/" + player.playerId + "/respawn", new WWWForm(), RespawnSuccess);

	}
	
	void RespawnSuccess (Hashtable response, object receiver) {
		Debug.Log ("Respawn success ");
		player.SyncAttributes((string)response["raw"]);
	}

/* :shields, :hull, :fuel, :fuel_burn, :speed,

                     :ammo, :ammo_burn, :cooldown,
                     :ammo_velocity, :ammo_duration, :ammo_damage] */

}
