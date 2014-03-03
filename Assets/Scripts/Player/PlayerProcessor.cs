using UnityEngine;
using System.Collections;

public class PlayerProcessor : APIBehaviour {

	public int playerId;
	public string playerName;

	APIObject lastAPIObject;

	public StatCollection stats;
	public ResourceCollection resources;

	public FuelProcessor fuelProcessor;

	void Start () {
		fuelProcessor = GetComponent<FuelProcessor>();
	}

	public void ParseAPIObject (APIObject o) {
		Debug.Log ("Parsing api object " + o);
		lastAPIObject = o;
		playerId = lastAPIObject.id;
		playerName = lastAPIObject.name;
		stats = o.stats;
		resources = o.resources;
	}

	[RPC]
	public void SyncToClient () {
		Debug.Log ("Sync to client called, last API object is " + lastAPIObject);
		networkView.RPC("SyncFromServer", uLink.RPCMode.Owner, lastAPIObject.raw);
	}

}
