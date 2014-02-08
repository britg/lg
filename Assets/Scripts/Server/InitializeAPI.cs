using UnityEngine;
using System.Collections;

public class InitializeAPI : PersistenceRequest {

	void Start () {
		GetAuthToken();
	}

	void GetAuthToken () {
		Get("/servers", GetAuthTokenSuccess);
	}

	void GetAuthTokenSuccess (IDictionary response, GameObject receiver) {
		Debug.Log ("Get auth token success " + response);
		PersistenceRequest.authenticityToken = (string) response["authenticity_token"];
		Debug.Log("Persistence token is now " + PersistenceRequest.authenticityToken);
	}
}
