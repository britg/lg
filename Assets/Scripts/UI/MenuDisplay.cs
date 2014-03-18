using UnityEngine;
using System.Collections;

public class MenuDisplay : DisplayBehaviour {

	public GameObject menuGUI;
	public GameObject labelsGUI;
	public bool allowMenu = false;

	void Update () {
		DetectInput();
	}

	void DetectInput () {
		if (allowMenu && Input.GetKeyDown(KeyCode.Escape)) {
			ToggleMenu();
		}
	}

	void ToggleMenu () {
		if (menuGUI.activeSelf) {
			DeactivateMenu();
		} else {
			ActivateMenu();
		}
	}

	void ActivateMenu () {
		NotificationCenter.PostNotification(this, LG.n_showMenu);
		labelsGUI.SetActive(false);
		menuGUI.SetActive(true);
	}

	void DeactivateMenu () {
		NotificationCenter.PostNotification(this, LG.n_hideMenu);
		labelsGUI.SetActive(true);
		menuGUI.SetActive(false);
	}

	public void OnDisconnectButtonPress () {
		DeactivateMenu();
		uLink.Network.Disconnect();
	}

	public void OnRespawnButtonPress () {
		DeactivateMenu();
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
