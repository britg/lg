using UnityEngine;
using System.Collections;

public class MenuController : ControllerBehaviour {

	public MenuDisplay menuDisplay;
	public bool allowMenu = false;

	protected override void OnMenuDown () {
		if (allowMenu) {
			ShowMenu();
		}
	}

	protected override void OnMenuMenuDown () {
		HideMenu();
	}

	void ShowMenu () {
		menuDisplay.ActivateMenu();
		NotificationCenter.PostNotification(this, LG.n_showMenu);
	}

	void HideMenu () {
		menuDisplay.DeactivateMenu();
		NotificationCenter.PostNotification(this, LG.n_hideMenu);
	}

	void uLink_OnConnectedToServer () {
		allowMenu = true;
	}

	void uLink_OnDisconnectedFromServer () {
		allowMenu = false;
	}

	public void OnDisconnectButtonPress () {
		HideMenu();
		uLink.Network.Disconnect();
	}
	
	public void OnRespawnButtonPress () {
		HideMenu();
		player.RequestRespawn();
	}
	
	public void OnQuitButtonPress () {
		Application.Quit();
	}

}
