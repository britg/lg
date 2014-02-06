using UnityEngine;
using System.Collections;
using SimpleJSON;

public class PlayerSpawner : PersistenceRequest {

	public Vector3 startRotation = new Vector3(0, 0, 0);

	public GameObject ownerPrefab;
	public GameObject proxyPrefab;
	public GameObject serverPrefab;

	private uLink.NetworkPlayer networkPlayer;
	private object[] initialData;

	public void StartPlayer (uLink.NetworkPlayer _networkPlayer) {
		networkPlayer = _networkPlayer;
		initialData = new object[1];
		initialData[0] = networkPlayer.loginData;
		string playerName;
		networkPlayer.loginData.TryRead(out playerName);

		GetPlayerAttributesByName(playerName);
	}

	public void GetPlayerAttributesByName (string playerName) {
		string endpoint = "/players/" + playerName + ".json";
		WWW request = new WWW(Endpoint(endpoint));
		onSuccess = GetPlayerAttributesByNameSuccess;
		onError = LogResponse;
		StartCoroutine(Request(request));
	}

	void GetPlayerAttributesByNameSuccess (string response, GameObject receiver) {
		Debug.Log ("Get player attribtues by name success: " + response);
		JSONNode serverAttr = JSON.Parse(response);
		Vector3 startPosition = new Vector3(serverAttr["x"].AsFloat, serverAttr["y"].AsFloat, serverAttr["z"].AsFloat);
		Quaternion rotation = Quaternion.Euler(startRotation);
		uLink.Network.Instantiate(networkPlayer, 
			                        proxyPrefab, 
			                        ownerPrefab, 
			                        serverPrefab, 
			                        startPosition, rotation, 0, initialData);
	}
}
