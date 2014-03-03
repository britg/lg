using UnityEngine;
using System.Collections;

public class LGMonoBehaviour : uLink.MonoBehaviour {

	protected GameObject projectileGrouping;
	protected GameObject projectile;
	protected FloatingTextController notifier;

	Player _player;
	protected Player player {
		get {
			if (_player == null) {
				_player = thePlayer().GetComponent<Player>();
			} 
			return _player;
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
		if (gameObject.name == clientPlayerName || gameObject.name == clientServerName) {
			return gameObject;
		} else {
			return GameObject.Find (clientPlayerName);
		}
	}

	protected float AngleDiff (Vector3 v1, Vector3 v2) {
	    float angle = Vector3.Angle(v1, v2);
	    float sign = Mathf.Sign(Vector3.Dot(-Vector3.forward, Vector3.Cross(v1, v2)));
	    return angle*sign;
	}

	protected void AssignNotifier () {
		notifier = GameObject.Find ("Notifications").GetComponent<FloatingTextController>();
	}

}
