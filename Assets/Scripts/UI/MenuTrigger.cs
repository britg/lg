using UnityEngine;
using System.Collections;

public class MenuTrigger : LGMonoBehaviour {

	public GameObject menuGUI;
	public bool allowMenu = false;

	void Update () {
		DetectInput();
	}

	void DetectInput () {
		if (allowMenu && Input.GetKeyDown(KeyCode.Escape)) {
			menuGUI.SetActive(!menuGUI.activeSelf);
		}
	}

	public void OnDisconnectButtonPress () {
		menuGUI.SetActive(false);
		uLink.Network.Disconnect();
	}

	public void OnRespawnButtonPress () {
		menuGUI.SetActive(false);
		player.RequestRespawn();
	}

	public void OnQuitButtonPress () {
		Application.Quit();
	}

	void uLink_OnConnectedToServer () {
		allowMenu = true;
	}

	void uLink_OnDisconnectedFromServer () {
		allowMenu = false;
	}
}
