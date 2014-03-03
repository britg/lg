using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : LGMonoBehaviour {

	public bool isOwner = false;
	public string playerName = "Player";
	public int playerId;

	[HideInInspector]
	public StatCollection stats;
	[HideInInspector]
	public ResourceCollection resources;
	[HideInInspector]
	public FuelController fuelController;

	void Start () {
		if (isOwner) {
			AssignNotifier();
		}

		fuelController = GetComponent<FuelController>();
	}

	void uLink_OnNetworkInstantiate (uLink.NetworkMessageInfo info) {
		info.networkView.initialData.TryRead<string>(out playerName);
		info.networkView.initialData.TryRead<int>(out playerId);
		SetLabel();
		networkView.RPC ("SyncToClient", uLink.RPCMode.Server);
	}

	void SetLabel () {
		TextMesh nameLabel = transform.Find("Nametag").GetComponent<TextMesh>();
		nameLabel.text = playerName;
	}
	
	void AnnounceLoaded() {
		Hashtable notificationData = new Hashtable();
		notificationData[LG.n_playerKey] = gameObject;
		NotificationCenter.PostNotification(this, LG.n_playerLoaded, notificationData);
	}

	[RPC] 
	void SyncFromServer (string rawAPIObject) {
		APIObject apiPlayer = new APIObject(rawAPIObject);
		stats = apiPlayer.stats;
		resources = apiPlayer.resources;
		NotificationCenter.PostNotification(this, LG.n_playerStatsLoaded);
	}

	[RPC]
	void SyncUpdateFromServer (string[] statArr) {
		Debug.Log ("Syncing stat array from server ");
		for (int i = 0; i < statArr.Length; i+=2) {
			string statName = statArr[i];
			float statValue;
			float.TryParse(statArr[i+1], out statValue);
			stats.Set (statName, statValue);
		}
	}

	public void RequestRespawn () {
		networkView.RPC ("Respawn", uLink.RPCMode.Server);
	}

	public float stat (string name) {
		return (float)stats.Get(name).value;
	}

}
