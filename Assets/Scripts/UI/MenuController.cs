using UnityEngine;
using System.Collections;

public class MenuController : ControllerBehaviour {

	public MenuDisplay menuDisplay;
	public bool allowMenu = false;

	protected override void OnMenuDown () {
		if (allowMenu) {
			menuDisplay.ActivateMenu();
			NotificationCenter.PostNotification(this, LG.n_showMenu);
		}
	}

	protected override void OnMenuMenuDown () {
		menuDisplay.DeactivateMenu();
		NotificationCenter.PostNotification(this, LG.n_hideMenu);
	}

	void uLink_OnConnectedToServer () {
		allowMenu = true;
	}

	void uLink_OnDisconnectedFromServer () {
		allowMenu = false;
	}

}
