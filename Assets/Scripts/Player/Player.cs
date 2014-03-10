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

	public GameObject nameLabel;


	void Start () {
		if (isOwner) {
			AssignNotifier();
		}

		fuelController = GetComponent<FuelController>();
	}

	void uLink_OnNetworkInstantiate (uLink.NetworkMessageInfo info) {
		info.networkView.initialData.TryRead<string>(out playerName);
		info.networkView.initialData.TryRead<int>(out playerId);
		networkView.RPC ("SyncToClient", uLink.RPCMode.Server);
	}

	void SetLabel () {
		LabelDisplay label = GetComponent<LabelDisplay>();
		label.label.text = playerName;
	}
	
	void AnnounceLoaded() {
		Hashtable notificationData = new Hashtable();
		notificationData[LG.n_playerKey] = gameObject;
		NotificationCenter.PostNotification(this, LG.n_playerLoaded, notificationData);
	}

	[RPC] 
	void SyncFromServer (string rawAPIObject) {
		APIObject apiPlayer = new APIObject(rawAPIObject);
		transform.position = apiPlayer.position;
		transform.rotation = apiPlayer.quaternion;
		transform.localScale = apiPlayer.scale;
		stats = apiPlayer.stats;
		resources = apiPlayer.resources;
		NotificationCenter.PostNotification(this, LG.n_playerStatsLoaded);
		SetLabel();
		LoadModules();
	}

	[RPC]
	void SyncStatsUpdateFromServer (string[] statArr) {
		for (int i = 0; i < statArr.Length; i+=2) {
			string statName = statArr[i];
			float statValue;
			float.TryParse(statArr[i+1], out statValue);
			stats.Set (statName, statValue);
		}
	}

	[RPC]
	void SyncResourcesUpdateFromServer (string[] resourceArr) {
		for (int i = 0; i < resourceArr.Length; i+=2) {
			string resourceName = resourceArr[i];
			int resourceValue;
			int.TryParse(resourceArr[i+1], out resourceValue);
			resources.Set (resourceName, resourceValue);
		}
	}

	public void RequestRespawn () {
		networkView.RPC ("Respawn", uLink.RPCMode.Server);
	}

	public float stat (string name) {
		return (float)stats.Get(name).value;
	}

	public int resource (string name) {
		return (int)resources.Get(name).value;
	}

	void LoadModules () {
		LoadWeapon ();
	}

	void LoadWeapon () {
		// TEMP
		GameObject weaponPrefab = (GameObject) Resources.Load ("RailGun");
		GameObject weapon = (GameObject) Instantiate(weaponPrefab);
		weapon.transform.parent = transform;
		weapon.transform.localPosition = Vector3.zero;
		NotificationCenter.PostNotification(this, LG.n_registerWeapon, iTween.Hash("weapon", weapon));
	}

}
