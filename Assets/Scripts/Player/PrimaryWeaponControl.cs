using UnityEngine;
using System.Collections;

public class PrimaryWeaponControl : LGMonoBehaviour {

	private bool isFiring = false;
	private Vector3 worldLookPoint;

	void Start () {
		Debug.Log ("Child start");
		InitProjectiles();
		AssignPlayerAttributes();
		LoadPrimaryWeapon();
	}

	void LoadPrimaryWeapon () {

	}
	
	// Update is called once per frame
	void Update () {
		DetectFireInput();
	}
	
	void DetectFireInput () {
		if (Input.GetButtonDown("Fire1")) {
			networkView.UnreliableRPC("StartFiring", uLink.RPCMode.Server);
			StartFiring ();
		}
		
		if (Input.GetButtonUp ("Fire1")) {
			networkView.UnreliableRPC("StopFiring", uLink.RPCMode.Server);
			StopFiring ();
		}
	}
	
	[RPC]
	void StartFiring () {
		if (isFiring) {
			return;
		}
		isFiring = true;
		InvokeRepeating("Fire", 0f, playerAttributes.weaponAttributes.cooldown);
	}
	
	[RPC]
	void StopFiring () {
		isFiring = false;
		CancelInvoke();
	}
	
	void Fire () {
		SpawnProjectile();
	}
	
	void SpawnProjectile () {
		GetWorldLookPoint();
		GameObject _projectile = (GameObject)Instantiate (projectile, transform.position, transform.rotation);
		float angleDiffLook = AngleDiff(transform.up, worldLookPoint);
		Vector3 angles = projectile.transform.eulerAngles;
		angles.z -= angleDiffLook;
		_projectile.transform.eulerAngles = angles;
		_projectile.transform.parent = projectileGrouping.transform;
	}

	void GetWorldLookPoint () {
		TopdownLook topdownLook = GetComponent<TopdownLook>();
		worldLookPoint = topdownLook.worldLookPoint;
	}

}
