using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpawner : PersistenceRequest {

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

	void RegisterPlayerSuccess (Hashtable serverAttr, object receiver) {
		object[] initialData = new object[2];

		initialData[0] = serverAttr["name"];
		initialData[1] = System.Convert.ToInt32(serverAttr["id"]);

		Vector3 startPosition = Vector3.zero;
		Quaternion rotation = new Quaternion();
		Vector3 angles = rotation.eulerAngles;
		float.TryParse(serverAttr["x"].ToString(), out startPosition.x);
		float.TryParse(serverAttr["y"].ToString(), out startPosition.y);
		float.TryParse(serverAttr["z"].ToString(), out angles.z);
		rotation.eulerAngles = angles;

		GameObject serverPlayer = uLink.Network.Instantiate((uLink.NetworkPlayer)receiver, 
									                        proxyPrefab, 
									                        ownerPrefab, 
									                        serverPrefab, 
									                        startPosition, rotation, 0, initialData);
		serverPlayer.transform.eulerAngles = angles;
		PlayerProcessor playerProcessor = serverPlayer.GetComponent<PlayerProcessor>();
		playerProcessor.SetStats(serverAttr);
	}

}
