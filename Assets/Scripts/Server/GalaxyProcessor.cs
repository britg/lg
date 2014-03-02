using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using uLink;

public class GalaxyProcessor : APIBehaviour {

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
		Get("/spawns", GetSpawnsSuccess);
	}

	void GetSpawnsSuccess (APIResponse response) {
		List<object> objects = (List<object>)response["objects"];
		WorldObject.PlaceObjects(objects);
		NotificationCenter.PostNotification(this, LG.n_worldObjectsSpawned);
	}

	[RPC]
	void GetNearbyObjects (Vector3 pos, uLink.NetworkMessageInfo info) {
		Debug.Log ("Getting nearby objects for " + pos);
		string query = "&type=static&x=" + pos.x.ToString() + "&y=" + pos.y.ToString() + "&z=" + pos.z.ToString();
		Get ("/spawns", query, info.sender, GetNearbyObjectsSuccess);
	}

	void GetNearbyObjectsSuccess(APIResponse response) {
		uLink.NetworkPlayer sender = (uLink.NetworkPlayer)response.receiver;
		networkView.RPC("LoadNearbyObjects", sender, response.raw);
	}

}
