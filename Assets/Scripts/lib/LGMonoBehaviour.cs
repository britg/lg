using UnityEngine;
using System.Collections;

public class LGMonoBehaviour : uLink.MonoBehaviour {

	protected GameObject projectileGrouping;
	protected GameObject projectile;
	protected Player playerAttributes;
	protected FloatingTextController notifier;


	protected void InitProjectiles () {
		projectileGrouping = GameObject.Find("Projectiles");
		if (projectileGrouping == null) {
			projectileGrouping = (GameObject)Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
			projectileGrouping.name = "Projectiles";
		}
		projectile = (GameObject)Resources.Load("Projectile");
	}

	protected GameObject thePlayer () {
		GameObject test = GameObject.Find ("Player - Owner(Clone)");
		if (test == null) {
			return gameObject;
		}
		return test;
	}

	protected void AssignPlayerAttributes () {
		playerAttributes = thePlayer().GetComponent<Player>();
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
