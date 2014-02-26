using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using uLink;

public class GalaxyProcessor : PersistenceRequest {

	Hashtable prefabCache = new Hashtable();
	string allObjectsRaw;

	void uLink_OnNetworkInstantiate () {
		// Request to get all world objects (will need to be optimized later)
		// Loop through each and spawn them
		// Fault tolerant: must have a prefab available for the objects.
		GetWorldObjects();
	}

	void GetWorldObjects () {
		Get("/spawns", GetSpawnsSuccess);
	}

	void GetSpawnsSuccess (Hashtable response, GameObject receiver) {
		allObjectsRaw = (string)response["raw"];
		List<object> objects = (List<object>)response["objects"];
		foreach (var obj in objects) {

			Hashtable attributes = new Hashtable((IDictionary)obj);
			bool networked = (bool)attributes["networked"];

			if (networked) {

			} else {
				PlaceStaticObject(attributes);
			}
		}

		NotificationCenter.PostNotification(this, LG.n_worldObjectsSpawned);
	}

	void PlaceStaticObject (Hashtable attributes) {
		int worldObjectId 		= System.Convert.ToInt32(attributes["id"]);
		string name 			= (string)attributes["name"];
		Vector3 pos 		  	= WorldObject.ExtractPosition(attributes);

		GameObject cached = (GameObject)prefabCache[name];
		if (cached == null) {
			prefabCache[name] = (GameObject)Resources.Load (name);
			cached = (GameObject)prefabCache[name];
		}

		GameObject serverObj = (GameObject)Instantiate(cached, pos, Quaternion.identity);
		serverObj.transform.parent = transform;
		serverObj.SendMessage("AssignAttributes", attributes, SendMessageOptions.DontRequireReceiver);
	}

	void PlaceNetworkedObject (string name, int id, Hashtable attributes) {

	}


	[RPC]
	void GetNearbyObjects (Vector3 pos, uLink.NetworkMessageInfo info) {
		Debug.Log ("Getting nearby objects for " + pos);
		// Get objects near the player and RPC back to the player the list of objects
		Debug.Log ("Raw response " + allObjectsRaw);
		networkView.RPC("LoadNearbyObjects", info.sender, allObjectsRaw);
	}

}
