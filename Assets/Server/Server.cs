using UnityEngine;
using System.Collections;

public class Server : MonoBehaviour {

	void StartServer () {
		Debug.Log ("Starting server... ");
		MasterServer.ipAddress = "127.0.0.1";
		MasterServer.port = 23466;
		Network.natFacilitatorIP = "127.0.0.1";
		Network.natFacilitatorPort = 50005;
		Network.InitializeServer(200, 1919, !Network.HavePublicAddress());
		MasterServer.RegisterHost("Lonely Galaxy", "Foolish Aggro US 1", "http://foolishaggro.com");
	}

	void OnServerInitialized () {
		Debug.Log ("Server started!");
	}

	void Start () {
		if (isHeadless()) {
			StartServer();
		} else {
			ConnectToServer();
		}
	}

	void ConnectToServer () {
		Network.Connect("foolishaggro.com", 1919);
	}

	void OnConnectedToServer () {
		Debug.Log ("Connected to server!");
	}

	bool isHeadless () {
		return SystemInfo.graphicsDeviceID == 0;
	}

}
