using UnityEngine;
using System.Collections;

public class LGMonoBehaviour : uLink.MonoBehaviour {

	protected GameObject projectileGrouping;
	protected GameObject projectile;
	protected PlayerAttributes playerAttributes;

	protected void Start () {
		Debug.Log ("Parent start");
	}

	protected void InitProjectiles () {
		projectileGrouping = GameObject.Find("Projectiles");
		if (projectileGrouping == null) {
			projectileGrouping = (GameObject)Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);
			projectileGrouping.name = "Projectiles";
		}
		projectile = (GameObject)Resources.Load("Projectile");
	}

	protected void AssignPlayerAttributes () {
		playerAttributes = GetComponent<PlayerAttributes>();
	}

	protected float AngleDiff (Vector3 v1, Vector3 v2) {
	    float angle = Vector3.Angle(v1, v2);
	    float sign = Mathf.Sign(Vector3.Dot(-Vector3.forward, Vector3.Cross(v1, v2)));
	    return angle*sign;
	}

}
