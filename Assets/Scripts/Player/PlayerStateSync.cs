using UnityEngine;
using System.Collections;

public class PlayerStateSync : PersistenceRequest {

	public float repeatRate = 1f;

	void Start () {
		AssignPlayerAttributes();
		StartSync();
	}

	void StartSync () {
		InvokeRepeating("Sync", 1f, repeatRate);
	}

	void Sync () {
		Vector3 pos = transform.position;
		WWWForm formData = new WWWForm();
		formData.AddField("player[x]", pos.x.ToString());
		formData.AddField("player[y]", pos.y.ToString());
		formData.AddField("player[z]", pos.z.ToString());
		Put ("/players/" + playerAttributes.playerId, formData);
	}

}
