using UnityEngine;
using System.Collections;

public class AssignShip : MonoBehaviour {

	void Start () {
		AddShipToPlayer();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void  AddShipToPlayer() {
		GameObject ship = (GameObject)Resources.Load("StarterShip");
		GameObject playerShip = (GameObject)Instantiate(ship, Vector3.zero, Quaternion.identity);
		playerShip.transform.parent = gameObject.transform;
		playerShip.transform.localPosition = Vector3.zero;
	}
}
