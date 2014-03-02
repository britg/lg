using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using uLink;

public class Player : LGMonoBehaviour {

	public bool isOwner = false;
	public string playerName = "Player";
	public int playerId;

	public StatCollection stats;
	public ResourceCollection resources;

	void Start () {
		if (isOwner) {
			AssignNotifier();
		}
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
		Debug.Log ("Syncing raw API Object " + rawAPIObject);
		APIObject apiPlayer = new APIObject(rawAPIObject);
		stats = apiPlayer.stats;
		resources = apiPlayer.resources;
		NotificationCenter.PostNotification(this, LG.n_playerStatsLoaded);
	}

	public void RequestRespawn () {
		networkView.RPC ("Respawn", uLink.RPCMode.Server);
	}

	public bool hasEnoughFuel (float time) {
		return true;
	}

	public float stat (string name) {
		return (float)stats.Get(name).value;
	}

}
