using UnityEngine;
using System.Collections;

public class PlayerPersistence : APIBehaviour {

	public float saveInterval = 1f;

	void Start () {
		InvokeRepeating("SaveAll", saveInterval, saveInterval);
	}

	public WWWForm PlayerToFormData () {
		WWWForm data = new WWWForm();

		Vector3 position = transform.position;
		Vector3 rotation = transform.rotation.eulerAngles;
		Vector3 scale = transform.localScale;
		StatCollection stats = playerProcessor.stats;
		ResourceCollection resources = playerProcessor.resources;

		data.AddField("player[x]", position.x.ToString());
		data.AddField("player[y]", position.y.ToString());
		data.AddField("player[z]", position.z.ToString());
		
		data.AddField("player[rotation][x]", rotation.x.ToString());
		data.AddField("player[rotation][y]", rotation.y.ToString());
		data.AddField("player[rotation][z]", rotation.z.ToString());
		
		data.AddField("player[scale][x]", scale.x.ToString());
		data.AddField("player[scale][y]", scale.y.ToString());
		data.AddField("player[scale][z]", scale.z.ToString());
		
		foreach (DictionaryEntry pair in stats) {
			string k = "player[stats][" + pair.Key.ToString() + "]";
			string v = pair.Value.ToString();
			data.AddField(k, v);
		}

		foreach (DictionaryEntry pair in resources) {
			string k = "player[resources][" + pair.Key.ToString() + "]";
			string v = pair.Value.ToString();
			data.AddField(k, v);
		}

		return data;
	}

	public void SaveAll () {
		Put ("/players/" + playerProcessor.playerId, PlayerToFormData(), SaveAllSuccess);
	}

	public void SaveAllSuccess (APIResponse response) {
		Debug.Log ("Save all response " + response);
	}

}
