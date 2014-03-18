﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using uLink;

public class GalaxyProxy : LGMonoBehaviour {

	// Use this for initialization
	void Start () {
		WorldObject.galaxy = gameObject;
		NotificationCenter.AddObserver(this, LG.n_playerStatsLoaded);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPlayerStatsLoaded () {
		Debug.Log ("On player loaded " + player);
		GetCurrentSector();
	}

	void GetCurrentSector () {
		networkView.UnreliableRPC(GalaxyProcessor.Server_GetSector, uLink.RPCMode.Server, player.transform.position);
	}

	void GetNearbyObjects () {
		networkView.UnreliableRPC("GetNearbyObjects", uLink.RPCMode.Server, player.transform.position);
	}

	[RPC]
	void LoadSector (string sectorName) {
		if (LG.cache[sectorName] == null) {
			GameObject sector = (GameObject)Resources.Load (sectorName);
			LG.cache[sectorName] = (GameObject)Instantiate(sector);
		}
	}

	[RPC]
	void LoadNearbyObjects (string raw) {
		APIResponse response = new APIResponse(raw);
		List<APIObject> apiObjects = response.GetObjects();
		WorldObject.PlaceObjects(apiObjects);
		NotificationCenter.PostNotification(this, LG.n_worldObjectsSpawned);
	}

}
