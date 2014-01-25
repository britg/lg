using UnityEngine;
using System.Collections;

public class Server : MonoBehaviour {

	void Start () {

	}

	void StartServer () {
		Debug.Log ("Starting server... ");
		MasterServer.ipAddress = "127.0.0.1";
		MasterServer.port = 23466;
		Network.natFacilitatorIP = "127.0.0.1";
		Network.natFacilitatorPort = 50005;
		Network.InitializeServer(200, 1919, !Network.HavePublicAddress());
		Network.logLevel = NetworkLogLevel.Full;
		MasterServer.RegisterHost("Lonely Galaxy", "Foolish Aggro US 1", "http://foolishaggro.com");
	}

	void StartLocalServer () {
		Debug.Log ("Starting local server... ");
		Network.InitializeServer(200, 1919, false);
		Network.logLevel = NetworkLogLevel.Full;
	}

	void OnServerInitialized () {
		Debug.Log ("Server started!");

		if (Network.isServer) {
			Debug.Log ("Hello, I am the server");
		}

		if (!isHeadless()) {
			PlayerShouldSpawn();
		}
	}

	void PromptConnection () {
		NotificationCenter.PostNotification(this, LG.n_showServerConnectionUI);
	}

	public void ConnectClicked () {
		ConnectToServer();
	}

	public void CreateServerClicked () {
		StartLocalServer();
	}

	void ConnectToServer () {
		Network.Connect("127.0.0.1", 1919);
	}

	void OnConnectedToServer () {
		Debug.Log ("Connected to server!");
		PlayerShouldSpawn();
	}

	bool isHeadless () {
		return (CommandLineReader.GetCustomArgument("server") == "1");
	}

	bool isLocalServer () {
		return (CommandLineReader.GetCustomArgument("localServer") == "1");
	}

	void PlayerShouldSpawn() {
		NotificationCenter.PostNotification(this, LG.n_playerShouldSpawn);
	}

	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Clean up after player " +  player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

}
