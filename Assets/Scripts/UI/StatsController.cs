using UnityEngine;
using System.Collections;

public class StatsController : ControllerBehaviour {

	public GameObject statsObj;

	void uLink_OnConnectedToServer () {
		statsObj.SetActive(true);
	}
	
	void uLink_OnDisconnectedFromServer(uLink.NetworkDisconnection mode) {
		statsObj.SetActive(false);
	}

	protected override void OnControllerShowMenu () {
		statsObj.SetActive (false);
	}

	protected override void OnControllerHideMenu () {
		statsObj.SetActive (true);
	}
}
