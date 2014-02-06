using UnityEngine;
using System.Collections;

public class PlayerStateSync : PersistenceRequest {

	public float repeatRate = 1f;

	void Start () {
		StartSync();
	}

	void StartSync () {
		InvokeRepeating("Sync", 1f, repeatRate);
	}

	void Sync () {
		Debug.Log ("Sync");
	}

	public void SetPlayerPosition (Vector3 pos) {
		string endpoint = Endpoint("/players/" + "test" + ".json");
		WWWForm formData = new WWWForm();
		formData.AddField("_method", "PUT");
		formData.AddField("player[x]", pos.x.ToString());
		formData.AddField("player[y]", pos.y.ToString());
		formData.AddField("player[z]", pos.z.ToString());
		WWW request = new WWW(endpoint, formData);
		StartCoroutine(Request(request));
	}
	
}
