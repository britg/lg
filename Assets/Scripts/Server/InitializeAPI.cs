using UnityEngine;
using System.Collections;
using uLink;

public class InitializeAPI : PersistenceRequest {

	public GameObject galaxyServer;
	public GameObject galaxyProxy;

	void Start () {
		GetAuthToken();
		InvokeRepeating("GetAuthToken", 60f, 60f);
		BuildGalaxy();
	}

	void GetAuthToken () {
		Get("/servers", "v=" + LG.version, GetAuthTokenSuccess);
	}

	void GetAuthTokenSuccess (IDictionary response, object receiver) {
		PersistenceRequest.authenticityToken = (string) response["authenticity_token"];
		Debug.Log(System.DateTime.Now.ToString("MM/dd/yyyy h:mm:ss ") + " Heartbeat " + PersistenceRequest.authenticityToken);
	}

	void BuildGalaxy () {
		uLink.Network.Instantiate( galaxyProxy, galaxyServer, Vector3.zero, Quaternion.identity, 0 );
	}
}
