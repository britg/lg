using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldObjectSpawner : PersistenceRequest {

	void uLink_OnServerInitialized () {
		// Request to get all world objects (will need to be optimized later)
		// Loop through each and spawn them
		// Fault tolerant: must have a prefab available for the objects.
		GetWorldObjects();
	}

	void GetWorldObjects () {
		Get("/spawns", GetSpawnsSuccess);
	}

	void GetSpawnsSuccess (Hashtable response, GameObject receiver) {
//		Debug.Log("Get spawns success " + response["objects"].GetType());

		// Instantiate a bunch of objects
		List<object> objects = (List<object>)response["objects"];
		foreach (var obj in objects) {
//			Debug.Log ("Object is " + obj);
			IDictionary dict = (IDictionary)obj;
			string name = "";
			Vector3 startPosition = Vector3.zero;
			string raw = "";
			foreach (DictionaryEntry prop in dict) {
//				Debug.Log ("key: " + prop.Key + " " + " value: " + prop.Value);
				if (prop.Key.ToString() == "template") {
					name = (string)prop.Value;
				}
				if (prop.Key.ToString() == "x") {
					float.TryParse(prop.Value.ToString(), out startPosition.x);
				}
				if (prop.Key.ToString() == "y") {
					float.TryParse(prop.Value.ToString(), out startPosition.y);
				}
				if (prop.Key.ToString() == "raw") {
					raw = (string)prop.Value;
				}
			}

//			Debug.Log ("Need to instantiate: " + name);
			GameObject proxy = (GameObject)Resources.Load (name + " - Proxy");
			GameObject server = (GameObject)Resources.Load (name + " - Server");
			GameObject serverObj = uLink.Network.Instantiate( proxy, server, 
			                                                  startPosition, Quaternion.identity, 0);
			serverObj.SendMessage("AssignAttributes", raw, SendMessageOptions.DontRequireReceiver);
		}

		// Notify the system that objects have been instantiated
		NotificationCenter.PostNotification(this, LG.n_worldObjectsSpawned);
	}

}
