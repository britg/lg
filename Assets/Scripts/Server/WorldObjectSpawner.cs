using UnityEngine;
using System.Collections;

public class WorldObjectSpawner : PersistenceRequest {

	void uLink_OnServerInitialized () {
		// Request to get all world objects (will need to be optimized later)
		// Loop through each and spawn them
		// Fault tolerant: must have a prefab available for the objects.

	}

	void GetSpawns () {
		Get("/spawns", GetSpawnsSuccess);
	}

	void GetSpawnsSuccess (Hashtable response, GameObject receiver) {
		Debug.Log("Get spawns success");
	}

}
