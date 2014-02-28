using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : LGMonoBehaviour {

	public bool isOwner = false;
	public string playerName = "Player";
	public int playerId;

	public StatCollection stats;

	void Start () {
		if (isOwner) {
			AssignNotifier();
		}
	}

	void uLink_OnNetworkInstantiate (uLink.NetworkMessageInfo info) {
		info.networkView.initialData.TryRead<string>(out playerName);
		info.networkView.initialData.TryRead<int>(out playerId);
		SetLabel();
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
	public void SendStats () {
		Debug.Log ("stats serialization is " + stats.toRPCSerialization());
//		networkView.RPC("ReceiveStats", uLink.RPCMode.Owner, stats.toRPCSerialization());
	}

	[RPC] 
	void ReceiveStats (string[] statsArr) {
		Debug.Log ("Syncing stats serverStats " + statsArr);
	}

	[RPC]
	void AddResources (Resource[] resources) {
//		notifier.Notify (gameObject, toAdd.ToFloatingText());
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
