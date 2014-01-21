using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

	public GameObject playerPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnConnectedToServer () {
		SpawnPlayer();
	}

	private void SpawnPlayer () {
		Network.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, 0);
	}
}
