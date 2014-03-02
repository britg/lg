using UnityEngine;
using System.Collections;

public class PlayerProcessor : APIBehaviour {

	APIObject lastAPIObject;

	public StatCollection stats;
	public ResourceCollection resources;

	public void ParseAPIObject (APIObject o) {
		Debug.Log ("Parsing api object " + o);
		lastAPIObject = o;
		stats = o.stats;
		resources = o.resources;
	}

	[RPC]
	public void SyncToClient () {
		Debug.Log ("Sync to client called, last API object is " + lastAPIObject);
		networkView.RPC("SyncFromServer", uLink.RPCMode.Owner, lastAPIObject.raw);
	}

}
