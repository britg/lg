using UnityEngine;
using System.Collections;

public class ProjectileLauncher : LGMonoBehaviour {

	public GameObject projectilePrefab;
	public int projectilesPerVolley = 3;
	public float volleyDelay = 0.1f;
	public Transform target;

	public void Fire (Transform _target) {
		target = _target;
		FireVolley ();
	}

	public void FireVolley () {
		for (int i = 0; i < projectilesPerVolley; i++) {
			Invoke("FireProjectile", volleyDelay * i);
		}
	}

	public void FireProjectile () {
		GameObject p = (GameObject) Instantiate(projectilePrefab, transform.position, Quaternion.identity);
		p.SendMessage("SetTarget", target);
	}
}
