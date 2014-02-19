using UnityEngine;
using System.Collections;

public class InitializeAPI : PersistenceRequest {

	void Start () {
		GetAuthToken();
		InvokeRepeating("GetAuthToken", 60f, 60f);
	}

	void GetAuthToken () {
		Get("/servers", "v=" + LG.version, GetAuthTokenSuccess);
	}

	void GetAuthTokenSuccess (IDictionary response, GameObject receiver) {
		PersistenceRequest.authenticityToken = (string) response["authenticity_token"];
		Debug.Log("Heartbeat " + PersistenceRequest.authenticityToken);
	}
}
