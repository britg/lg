using UnityEngine;
using System.Collections;
using uLink;

public class MobSpawner : APIBehaviour {

	Transform mobBucket;

	void Awake () {
		mobBucket = (new GameObject("Mobs")).transform;
	}

	void Start () {
		NotificationCenter.AddObserver(this, LG.n_worldObjectsSpawned);
	}

	void OnWorldObjectsSpawned () {
		CreateMobBucket();
		if (ShouldSpawnMob()) {
			SpawnRandomMob();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void CreateMobBucket () {
		mobBucket.position = Vector3.zero;
	}

	bool ShouldSpawnMob () {
		return false;
//		return mobBucket.childCount > 0;
	}

	void SpawnRandomMob () {
//		Vector3 startPosition = Vector3.zero;
//		GameObject serverMob = uLink.Network.Instantiate( enemyProxy, enemyServer, 
//		                                                  startPosition, Quaternion.identity, 0);
//		serverMob.transform.parent = mobBucket;

		// Request to server for a specific type of mob
		WWWForm postData = new WWWForm();
		postData.AddField("spawn[name]", "AncientDrone");
		postData.AddField("spawn[x]", "0.0");
		postData.AddField("spawn[y]", "0.0");
		postData.AddField ("customizations[health]", "100");
		Post("/spawns", postData, SpawnRequestSuccess, SpawnRequestError);
	}

	void SpawnRequestSuccess (APIResponse response) {
		Debug.Log ("Spawn request success");

	}

	void SpawnRequestError (APIResponse response) {
		Debug.Log ("Spawn request error");
	}

}
