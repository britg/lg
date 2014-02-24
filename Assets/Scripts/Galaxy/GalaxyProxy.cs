using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using uLink;

public class GalaxyProxy : LGMonoBehaviour {

	Hashtable prefabCache = new Hashtable();

	// Use this for initialization
	void Start () {
		NotificationCenter.AddObserver(this, LG.n_playerLoaded);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPlayerLoaded () {
		Debug.Log ("On player loaded");
		GetNearbyObjects();
	}

	void GetNearbyObjects () {
		networkView.UnreliableRPC("GetNearbyObjects", uLink.RPCMode.Server, thePlayer().transform.position);
	}

	[RPC]
	void LoadNearbyObjects (string raw) {
		Hashtable response = MiniJSON.Json.Hashtable(raw);
		// Instantiate a bunch of objects
		List<object> objects = (List<object>)response["objects"];
		foreach (var obj in objects) {
			//			Debug.Log ("Object is " + obj);
			IDictionary dict = (IDictionary)obj;
			string name = "";
			Vector3 startPosition = Vector3.zero;
			string obj_raw = "";
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
					obj_raw = (string)prop.Value;
				}
			}
			
			//			Debug.Log ("Need to instantiate: " + name);
			GameObject cached = (GameObject)prefabCache[name];
			if (cached == null) {
				prefabCache[name] = (GameObject)Resources.Load (name);
				cached = (GameObject)prefabCache[name];
			}
			
			GameObject clientObj = (GameObject)Instantiate( cached, startPosition, Quaternion.identity);
			clientObj.transform.parent = transform;
		}
	}

}
