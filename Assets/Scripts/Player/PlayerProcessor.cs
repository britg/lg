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
		stats = o.stats;
		resources = o.resources;
	}

	[RPC]
	public void SyncToClient () {
		networkView.RPC("SyncFromServer", uLink.RPCMode.Owner, lastAPIObject.raw);
	}

	public void SyncToClientInterval () {
		if (stats.ShouldSync()) {
			networkView.UnreliableRPC("SyncUpdateFromServer", uLink.RPCMode.Owner, stats.StatsToSync());
			stats.FlushStatsToSync();
		}
	}

	[RPC]
	void Respawn () {
		Post ("/players/" + playerId + "/respawn", PlayerDefaults.toFormData(), RespawnSuccess);
	}

	void RespawnSuccess (APIResponse response) {
		ParseAPIObject(response.GetObject());
		SyncToClient();
	}

	public float stat (string name) {
		return (float)stats.Get(name).value;
	}

}
