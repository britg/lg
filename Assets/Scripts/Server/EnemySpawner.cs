using UnityEngine;
using System.Collections;
using uLink;

public class EnemySpawner : LGMonoBehaviour {

	public GameObject enemyProxy;
	public GameObject enemyServer;

	Transform enemyBucket;

	void uLink_OnServerInitialized () {
		CreateEnemyBucket();
		Spawn();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void CreateEnemyBucket () {
		enemyBucket = (new GameObject("Mobs")).transform;
		enemyBucket.position = Vector3.zero;
	}

	void Spawn () {
		Vector3 startPosition = Vector3.zero;
		GameObject serverMob = uLink.Network.Instantiate( enemyProxy, enemyServer, 
		                                                  startPosition, Quaternion.identity, 0);
		serverMob.transform.parent = enemyBucket;
	}
}
