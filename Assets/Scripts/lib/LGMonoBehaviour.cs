using UnityEngine;
using System.Collections;

public class LGMonoBehaviour : uLink.MonoBehaviour {

	protected GameObject projectileGrouping;
	protected GameObject projectile;
	protected FloatingTextController notifier;

	Player _player;
	protected Player player {
		get {
			if (LG.player == null) {
				LG.player = thePlayer().GetComponent<Player>();
			} 
			return LG.player;
		}
	}

	PlayerProcessor _playerProcessor;
	protected PlayerProcessor playerProcessor {
		get {
			if (_playerProcessor == null) {
				_playerProcessor = thePlayer ().GetComponent<PlayerProcessor>();
			}
			return _playerProcessor;
		}
	}

	protected void InitProjectiles () {
		projectileGrouping = GameObject.Find("Projectiles");
		if (projectileGrouping == null) {
			projectileGrouping = (GameObject)Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
			projectileGrouping.name = "Projectiles";
		}
		projectile = (GameObject)Resources.Load("Projectile");
	}

	protected GameObject thePlayer () {
		string clientPlayerName = "Player - Owner(Clone)";
		string clientServerName = "Player - Server(Clone)";
		string explorerName = "ExploreAnchor";
		if (gameObject.name == clientPlayerName || gameObject.name == clientServerName || gameObject.name == explorerName) {
			return gameObject;
		} else {
			GameObject go = GameObject.Find (clientPlayerName);
			if (go != null) {
				return go;
			}
			print ("Falling back to explorer " + GameObject.Find (explorerName));
			return GameObject.Find (explorerName);
		}
	}

	protected float AngleDiff (Vector3 v1, Vector3 v2) {
	    float angle = Vector3.Angle(v1, v2);
	    float sign = Mathf.Sign(Vector3.Dot(-Vector3.forward, Vector3.Cross(v1, v2)));
	    return angle*sign;
	}

}
