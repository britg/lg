using UnityEngine;
using System.Collections;

public class WeaponController : LGMonoBehaviour {

	private bool isFiring = false;
	private Vector3 worldLookPoint;

	void Start () {
		InitProjectiles();
		AssignPlayerAttributes();
	}

	// Update is called once per frame
	void Update () {
		if (playerAttributes.isOwner) {
			DetectFireInput();
		}
	}
	
	void DetectFireInput () {
		if (Input.GetButtonDown("Fire1")) {
			StartFiring ();
		}
		
		if (Input.GetButtonUp ("Fire1")) {
			StopFiring ();
		}
	}

	void GetWorldLookPoint () {
		TopdownLook topdownLook = GetComponent<TopdownLook>();
		worldLookPoint = topdownLook.worldLookPoint;
	}
	
	void StartFiring () {
		if (isFiring) {
			return;
		}
		isFiring = true;
		RepeatFiring ();
	}

	void RepeatFiring () {
		InvokeRepeating("Fire", 0.01f, playerAttributes.weaponAttributes.cooldown);
	}
	
	void StopFiring () {
		isFiring = false;
		CancelInvoke();
	}
	
	void Fire () {
		GetWorldLookPoint();
		if (playerAttributes.weaponAttributes.hasEnoughAmmo()) {
			FireAmmo (worldLookPoint);
			networkView.UnreliableRPC("FireAmmo", uLink.RPCMode.Server, worldLookPoint);
		}
	}

	[RPC]
	void FireAmmo (Vector3 _direction) {
		// server check
		if (!playerAttributes.weaponAttributes.hasEnoughAmmo()) {
			return;
		}

		playerAttributes.weaponAttributes.UseAmmo();
		SpawnProjectile(_direction);
		if (uLink.Network.isServer) {
			networkView.UnreliableRPC("SyncAmmo", uLink.RPCMode.Others, playerAttributes.weaponAttributes.ammo);
			networkView.UnreliableRPC("SpawnProjectile", uLink.RPCMode.AllExceptOwner, _direction);
		}
	}

	[RPC]
	void SpawnProjectile (Vector3 _direction) {
		GameObject _projectile = (GameObject)Instantiate (projectile, transform.position, transform.rotation);
		_projectile.transform.parent = projectileGrouping.transform;

		// Set the projectile attributes from player attributes
		Projectile projectileAttributes = _projectile.GetComponent<Projectile>();
		projectileAttributes.velocity = playerAttributes.weaponAttributes.velocity;
		projectileAttributes.life = playerAttributes.weaponAttributes.life;
		projectileAttributes.firedBy = transform;

		// Set the correct angle on the projectile
		float angleDiffLook = AngleDiff(transform.up, _direction);
		Vector3 angles = projectile.transform.eulerAngles;
		angles.z -= angleDiffLook - transform.eulerAngles.z;
		_projectile.transform.eulerAngles = angles;
	}

}
