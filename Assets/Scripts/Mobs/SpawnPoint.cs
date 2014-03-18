using UnityEngine;
using System.Collections;

public class SpawnPoint : LGMonoBehaviour {

	enum State {
		Spawned,
		Spawning
	}

	public float radius = 100f;
	public int mobCount = 1;
	public float respawnDelay = 30f;
	public GameObject serverPrefab;
	public GameObject clientPrefab;

	State currentState = State.Spawned;

	// Use this for initialization
	void Start () {
		if (uLink.Network.isServer) {
			InitialSpawn();
			InvokeRepeating("Check", 1f, 1f);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Check () {
		if (transform.childCount < mobCount && currentState != State.Spawning) {
			Invoke ("Spawn", respawnDelay);
			currentState = State.Spawning;
		}
	}

	void InitialSpawn () {
		for (int i = 0; i < mobCount; i++) {
			Spawn();
		}
	}

	void Spawn () {
		GameObject spawn = uLink.Network.Instantiate(clientPrefab, serverPrefab, RandomPosition(), Quaternion.identity, 0);
		spawn.transform.parent = transform;
		currentState = State.Spawned;
	}

	Vector3 RandomPosition () {
		Vector2 point = Random.insideUnitCircle*radius;
		Vector3 point3 = new Vector3(point.x, point.y, 0f);
		return point3;
	}
}
