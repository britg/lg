using UnityEngine;
using System.Collections;

public class PlayerProxy : LGMonoBehaviour {

	public string playerName = "Player";
	public int playerId;

	void Start () {
		Debug.Log("Player proxy start");
		SetLabel();
	}
	
	void uLink_OnNetworkInstantiate (uLink.NetworkMessageInfo info) {
		Debug.Log ("Player proxy on network instantiate");
		info.networkView.initialData.TryRead<string>(out playerName);
		info.networkView.initialData.TryRead<int>(out playerId);
	}
	
	void SetLabel () {
		Debug.Log ("Player proxy set label");
		LabelDisplay label = GetComponent<LabelDisplay>();
		label.label.text = playerName;
	}
}
