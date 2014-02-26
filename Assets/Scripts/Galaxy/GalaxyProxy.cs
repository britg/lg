using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using uLink;

public class GalaxyProxy : LGMonoBehaviour {

	// Use this for initialization
	void Start () {
		WorldObject.galaxy = gameObject;
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
		List<object> objects = (List<object>)response["objects"];
		WorldObject.PlaceObjects(objects);
	}

}
