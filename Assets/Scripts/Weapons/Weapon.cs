using UnityEngine;
using System.Collections;

public class Weapon : LGMonoBehaviour {

	public WeaponControlType controlType;
	public ProjectileType projectileType;
	public int ammoPerShot;
	public float shotDelay;
	public int shotsPerVolley;
	public float volleyDelay;


	public GameObject projectilePrefab;
	public float projectileLife;
	public float projectileVelocity;
	public bool destroyOnImpact;


	[HideInInspector]
	public Transform target;
	[HideInInspector]
	public Vector3 direction;

	public float Range {
		get {
			return projectileLife * projectileVelocity;
		}
	}

	public void Fire (Transform _target) {
		target = _target;
		FireVolley();
	}

	public void FireTowards (Vector3 _direction) {
		direction = _direction;
		FireVolley();
	}
	
	public void FireVolley () {
		for (int i = 0; i < shotsPerVolley; i++) {
			Invoke("FireProjectile", shotDelay * i);
		}
	}
	
	public void FireProjectile () {
		GameObject p = (GameObject) Instantiate(projectilePrefab, transform.position, Quaternion.identity);
		Projectile projectile = p.GetComponent<Projectile>();
		projectile.life = projectileLife;
		projectile.velocity = projectileVelocity;
		projectile.destroyOnImpact = destroyOnImpact;
		projectile.type = projectileType;
		projectile.target = target;
		projectile.direction = direction;
	}
}
