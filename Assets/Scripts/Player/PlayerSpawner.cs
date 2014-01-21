using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

	public GameObject playerPrefab;

	private GameObject player;

	void Awake () {
		NotificationCenter.AddObserver(this, LG.n_playerShouldSpawn);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPlayerShouldSpawn () {
		SpawnPlayer();
	}

	private void SpawnPlayer () {
		player = (GameObject)Network.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, 0);
		NotifyPlayerSpawned();
	}

	private void NotifyPlayerSpawned () {
		Hashtable nData = new Hashtable();
		nData[LG.n_playerKey] = player;
		NotificationCenter.PostNotification(this, LG.n_playerSpawned, nData);
	}
}
