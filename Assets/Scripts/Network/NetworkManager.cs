using UnityEngine;
using uLink;

public class NetworkManager : uLink.MonoBehaviour {

	private string playerNameKey = "player.name";

	public string host = "127.0.0.1";
	public int port = 7100;

	public GameObject serverConnectionGUI;
	public GameObject connectingGUI;
	public GameObject statsGUI;
	public UIInput playerNameInput;
	
	void Awake() {
		string playerName = PlayerPrefs.GetString(playerNameKey, "Enter Username");
		serverConnectionGUI.SetActive(true);
		playerNameInput.value = playerName;
	}

	public void OnConnectButtonPress () {
		Debug.Log ("On connect button pressed ");
		playerNameInput.Submit();
	}

	public void OnInputSubmit () {
		string selectedName = UIInput.current.value;
		Debug.Log ("Input submit "+ selectedName);
		PlayerPrefs.SetString(playerNameKey, selectedName);
		Connect (host, port, selectedName);
	}

	public void Connect(string _host, int _port, string playerName) {
		serverConnectionGUI.SetActive(false);
		connectingGUI.SetActive(true);
		uLink.Network.Connect(_host, _port, "", playerName);
	}

	public void uLink_OnFailedToConnect (uLink.NetworkConnectionError error) {
		serverConnectionGUI.SetActive(true);
		connectingGUI.SetActive(false);
	}

	void uLink_OnConnectedToServer () {
		serverConnectionGUI.SetActive(false);
		statsGUI.SetActive(true);
	}

	void uLink_OnDisconnectedFromServer(uLink.NetworkDisconnection mode) {
		serverConnectionGUI.SetActive(true);
		statsGUI.SetActive(false);
	}
}

