using UnityEngine;
using System.Collections;

public class PlayerProcessor : APIBehaviour {

	public int playerId;
	public string playerName;

	public float statSyncInterval = 0.2f;

	APIObject lastAPIObject;

	[HideInInspector]
	public StatCollection stats;
	[HideInInspector]
	public ResourceCollection resources;
	[HideInInspector]
	public FuelProcessor fuelProcessor;

	void Start () {
		fuelProcessor = GetComponent<FuelProcessor>();
		InvokeRepeating("SyncToClientInterval", statSyncInterval, statSyncInterval);
	}

	public void ParseAPIObject (APIObject o) {
		lastAPIObject = o;
		playerId = lastAPIObject.id;
		playerName = lastAPIObject.name;
		transform.position = lastAPIObject.position;
		transform.rotation = lastAPIObject.quaternion;
		transform.localScale = lastAPIObject.scale;
		stats = o.stats;
		resources = o.resources;
	}

	[RPC]
	public void SyncToClient () {
		Debug.Log ("syncing to client raw " + lastAPIObject.raw);
		networkView.RPC("SyncFromServer", uLink.RPCMode.Owner, lastAPIObject.raw);
	}

	public void SyncToClientInterval () {
		if (stats.ShouldSync()) {
			networkView.UnreliableRPC("SyncStatsUpdateFromServer", uLink.RPCMode.Owner, stats.StatsToSync());
			stats.FlushStatsToSync();
		}

		if (resources.ShouldSync()) {
			networkView.UnreliableRPC("SyncResourcesUpdateFromServer", uLink.RPCMode.Owner, resources.ResourcesToSync());
			resources.FlushResourcesToSync();
		}
	}

	[RPC]
	void Respawn () {
		WWWForm f = new WWWForm();
		f.AddField("player[name]", playerName);
		Post ("/players/" + playerId + "/respawn", f, RespawnSuccess);
	}

	void RespawnSuccess (APIResponse response) {
		ParseAPIObject(response.GetObject());
		SyncToClient();
	}

	public float stat (string name) {
		return (float)stats.Get(name).value;
	}

}
