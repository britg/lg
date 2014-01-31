using UnityEngine;
using System.Collections;

public class ServerDamageReceiver : LGMonoBehaviour {

	void Awake () {
		NotificationCenter.AddObserver(this, LG.n_playerHit);
	}

	void OnPlayerHit (Notification n) {
		Hashtable nData = n.data;

		GameObject shipMesh = (GameObject)nData["hit"];
		GameObject hit  = shipMesh.transform.parent.transform.parent.gameObject;

		if (hit == gameObject) {

			GameObject shooter = (GameObject)nData["shooter"];
			PlayerAttributes shooterAttributes = shooter.GetComponent<PlayerAttributes>();
			float damage = shooterAttributes.weaponAttributes.damage;

			Debug.Log ("Player hit me:" + hit + " with damage " + damage);
			networkView.UnreliableRPC("SyncDamage", uLink.RPCMode.All, damage);
		}
	}

}
