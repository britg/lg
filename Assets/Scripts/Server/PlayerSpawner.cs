﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpawner : PersistenceRequest {

	public GameObject ownerPrefab;
	public GameObject proxyPrefab;
	public GameObject serverPrefab;

	private uLink.NetworkPlayer networkPlayer;

	public void StartPlayer (uLink.NetworkPlayer _networkPlayer) {
		networkPlayer = _networkPlayer;
		string playerName;
		networkPlayer.loginData.TryRead(out playerName);

		RegisterPlayer(playerName);
	}

	public void RegisterPlayer (string playerName) {
		WWWForm formData = new WWWForm();
		formData.AddField("player[name]", playerName);
		Post ("/players", formData, RegisterPlayerSuccess);
	}

	void RegisterPlayerSuccess (Hashtable serverAttr, object receiver) {
		IDictionary properties = (IDictionary)serverAttr["properties"];
		object[] initialData = new object[2];

		initialData[0] = serverAttr["name"];
		initialData[1] = System.Convert.ToInt32(serverAttr["id"]);

		Vector3 startPosition = Vector3.zero;
		Quaternion rotation = new Quaternion();
		Vector3 angles = rotation.eulerAngles;
		float.TryParse(properties["x"].ToString(), out startPosition.x);
		float.TryParse(properties["y"].ToString(), out startPosition.y);
		float.TryParse(properties["z"].ToString(), out angles.z);
		rotation.eulerAngles = angles;

		GameObject serverPlayer = uLink.Network.Instantiate(networkPlayer, 
									                        proxyPrefab, 
									                        ownerPrefab, 
									                        serverPrefab, 
									                        startPosition, rotation, 0, initialData);
		serverPlayer.transform.eulerAngles = angles;
		Debug.Log ("Server player is " + serverPlayer);
		Player playerAttributes = serverPlayer.GetComponent<Player>();
		playerAttributes.SyncAttributes((string)serverAttr["raw"]);
	}

}
