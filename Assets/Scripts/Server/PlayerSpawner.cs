using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpawner : APIBehaviour {

	public GameObject ownerPrefab;
	public GameObject proxyPrefab;
	public GameObject serverPrefab;

	public void StartPlayer (uLink.NetworkPlayer networkPlayer) {
		string playerName;
		networkPlayer.loginData.TryRead(out playerName);
		WWWForm formData = PlayerDefaults.toFormData();
		formData.AddField("player[name]", playerName);
		Post ("/players", formData, networkPlayer, RegisterPlayerSuccess);
	}

	void RegisterPlayerSuccess (APIResponse response) {
		object[] initialData = new object[2];
		APIObject apiPlayer = response.GetObject();

		Debug.Log ("API response is " + response);
		Debug.Log ("API player is " + apiPlayer.id);

		initialData[0] = apiPlayer.name;
		initialData[1] = apiPlayer.id;

		GameObject serverPlayer = uLink.Network.Instantiate((uLink.NetworkPlayer)response.receiver, 
									                        proxyPrefab, 
									                        ownerPrefab, 
									                        serverPrefab, 
									                        apiPlayer.position, apiPlayer.quaternion, 0, initialData);
		PlayerProcessor playerProcessor = serverPlayer.GetComponent<PlayerProcessor>();
		playerProcessor.ParseAPIObject(apiPlayer);
	}

}
