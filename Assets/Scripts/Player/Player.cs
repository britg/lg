using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : LGMonoBehaviour {

	public string playerName = "Player";
	public int playerId;

	[HideInInspector]
	public StatCollection stats;
	[HideInInspector]
	public ResourceCollection resources;
	[HideInInspector]
	public FuelController fuelController;

	public GameObject nameLabel;
	public string loadedWeaponName;

	void Start () {
		fuelController = GetComponent<FuelController>();
		networkView.RPC (PlayerProcessor.Server_SyncToClient, uLink.RPCMode.Server);
	}

	void uLink_OnNetworkInstantiate (uLink.NetworkMessageInfo info) {
		info.networkView.initialData.TryRead<string>(out playerName);
		info.networkView.initialData.TryRead<int>(out playerId);
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

	public static string Client_SyncFromServer = "SyncFromServer";
	[RPC] void SyncFromServer (string rawAPIObject) {
		APIObject apiPlayer = new APIObject(rawAPIObject);
		transform.position = apiPlayer.position;
		transform.rotation = apiPlayer.quaternion;
		transform.localScale = apiPlayer.scale;
		stats = apiPlayer.stats;
		resources = apiPlayer.resources;
		SetLabel();
		AnnounceLoadComplete();
	}

	public static string Client_SyncStatsUpdateFromServer = "SyncStatsUpdateFromServer";
	[RPC] void SyncStatsUpdateFromServer (string[] statArr) {
		for (int i = 0; i < statArr.Length; i+=2) {
			string statName = statArr[i];
			float statValue;
			float.TryParse(statArr[i+1], out statValue);
			stats.Set (statName, statValue);
		}
	}

	public static string Client_SyncResourcesUpdateFromServer = "SyncResourcesUpdateFromServer";
	[RPC] void SyncResourcesUpdateFromServer (string[] resourceArr) {
		for (int i = 0; i < resourceArr.Length; i+=2) {
			string resourceName = resourceArr[i];
			int resourceValue;
			int.TryParse(resourceArr[i+1], out resourceValue);
			resources.Set (resourceName, resourceValue);
		}
	}

	void AnnounceLoadComplete () {
		if (player != null) {
			NotificationCenter.PostNotification(this, LG.n_playerStatsLoaded);
		} else {
			Invoke ("AnnounceLoadComplete", 0f);
		}
	}

	public void RequestRespawn () {
		networkView.RPC (PlayerProcessor.Server_Respawn, uLink.RPCMode.Server);
	}

	public float stat (string name) {
		return (float)stats.Get(name).value;
	}

	public int resource (string name) {
		return (int)resources.Get(name).value;
	}

	public static string Client_LoadWeapon = "LoadWeapon";
	[RPC] void LoadWeapon (string weaponName) {
		UnloadWeapon();

		GameObject weaponPrefab = (GameObject) Resources.Load (weaponName);
		Weapon weapon = ((GameObject)Instantiate(weaponPrefab)).GetComponent<Weapon>();
		weapon.transform.parent = transform;
		weapon.transform.localPosition = Vector3.zero;
		string controlScriptName = weapon.controlType.ToString() + "WeaponController";
		WeaponController weaponController = gameObject.AddComponent(controlScriptName) as WeaponController;
		weaponController.weapon = weapon;
		NotificationCenter.PostNotification(this, LG.n_registerWeapon, LG.Hash("weapon", weapon));
	}

	void UnloadWeapon () {
		Destroy(GetComponent<WeaponController>());
	}
}
