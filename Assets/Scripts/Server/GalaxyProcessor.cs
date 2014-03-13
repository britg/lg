using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using uLink;

public class GalaxyProcessor : APIBehaviour {

	Hashtable sectorCache = new Hashtable();

	void Start () {
		WorldObject.galaxy = gameObject;
	}

	void uLink_OnNetworkInstantiate () {
		// Request to get all world objects (will need to be optimized later)
		// Loop through each and spawn them
		// Fault tolerant: must have a prefab available for the objects.
		GetWorldObjects();
	}

	void GetWorldObjects () {
//		Get("/spawns", GetSpawnsSuccess);
	}

	void GetSpawnsSuccess (APIResponse response) {
		List<APIObject> apiObjects = response.GetObjects();
		WorldObject.PlaceObjects(apiObjects);
		NotificationCenter.PostNotification(this, LG.n_worldObjectsSpawned);
	}

	public static string Server_GetSector = "GetSector";
	[RPC]
	void GetSector (Vector3 pos, uLink.NetworkMessageInfo info) {
		print ("Getting sector for " + pos);
		// Instantiate on server side for stuff
		string chosenSector = "SectorA";
		if (sectorCache[chosenSector] == null) {
			GameObject sector = (GameObject) Resources.Load (chosenSector);
			sectorCache[chosenSector] = sector;
			Instantiate (sector);
		}
		networkView.RPC ("LoadSector", info.sender, chosenSector);
	}

	[RPC]
	void GetServerNearbyObjects (Vector3 pos, uLink.NetworkMessageInfo info) {
		Debug.Log ("Getting nearby objects for " + pos);
		string query = "&type=static&x=" + pos.x.ToString() + "&y=" + pos.y.ToString() + "&z=" + pos.z.ToString();
		Get ("/spawns", query, info.sender, GetNearbyObjectsSuccess);
	}

	void GetNearbyObjectsSuccess(APIResponse response) {
		uLink.NetworkPlayer sender = (uLink.NetworkPlayer)response.receiver;
		networkView.RPC("LoadNearbyObjects", sender, response.raw);
	}

}
