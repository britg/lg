using UnityEngine;
using System.Collections;

public class AssignShip : uLink.MonoBehaviour {

	private uLink.NetworkPlayer player;
	public GameObject shipPrefab;

	void Start () {
	}

	void uLink_OnNetworkInstantiate (uLink.NetworkMessageInfo info) {
//		Debug.Log ("Assign ship on network instantiate!!!");
		player = info.networkView.owner;
		if (uLink.Network.isServer) {
			GetShip();
			networkView.UnreliableRPC("AddShipToPlayer", uLink.RPCMode.All, shipPrefab);
		}
	}

	void GetShip () {
		// TODO retrieve current ship from database
		// TEMP just assign the starter ship for now
	}

	[RPC]
	void AddShipToPlayer (GameObject _shipPrefab) {
		GameObject ship = (GameObject)Instantiate (shipPrefab, transform.position, transform.rotation);
		ship.transform.parent = transform;
	}

	void  NetworkAddShipToPlayer() {
//		Debug.Log ("Adding ship to player");
		uLink.Network.Instantiate(player, 
		                            shipPrefab, 
		                            shipPrefab, 
		                            shipPrefab, 
		                            gameObject.transform.position, 
		                            gameObject.transform.rotation, 
		                            0, null);

	}
}
