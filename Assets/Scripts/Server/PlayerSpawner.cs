using UnityEngine;
using System.Collections;

public class PlayerSpawner : PersistenceRequest {

	public Vector3 startRotation = new Vector3(0, 0, 0);

	public GameObject ownerPrefab;
	public GameObject proxyPrefab;
	public GameObject serverPrefab;

	private uLink.NetworkPlayer networkPlayer;
	private object[] initialData;

	public void StartPlayer (uLink.NetworkPlayer _networkPlayer) {
		networkPlayer = _networkPlayer;
		initialData = new object[2];
		initialData[0] = networkPlayer.loginData;
		string playerName;
		networkPlayer.loginData.TryRead(out playerName);

		RegisterPlayer(playerName);
	}

	public void RegisterPlayer (string playerName) {
		WWWForm formData = new WWWForm();
		formData.AddField("player[name]", playerName);
		Post ("/players", formData, RegisterPlayerSuccess);
	}

	void RegisterPlayerSuccess (IDictionary serverAttr, GameObject receiver) {
		Vector3 startPosition = new Vector3((float)(double)serverAttr["x"], (float)(double)serverAttr["y"], (float)(double)serverAttr["z"]);
		Quaternion rotation = Quaternion.Euler(startRotation);
		Debug.Log ("id is " + serverAttr["id"]);
		initialData[1] = System.Convert.ToInt32(serverAttr["id"]);
		uLink.Network.Instantiate(networkPlayer, 
			                        proxyPrefab, 
			                        ownerPrefab, 
			                        serverPrefab, 
			                        startPosition, rotation, 0, initialData);
	}
}
