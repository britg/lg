using UnityEngine;
using System.Collections;

public class PrimaryWeaponControl : LGMonoBehaviour {

	public Weapon primaryWeapon;

	private static string objectName = "PrimaryWeapon";
	private bool isFiring = false;
	private Vector3 worldLookPoint;

	void Start () {
		InitProjectiles();
		LoadPrimaryWeapon();
	}

	void LoadPrimaryWeapon () {
		foreach (Transform child in transform) {
			foreach (Transform child2 in child) {
				if (child2.gameObject.name == objectName) {
					primaryWeapon = child2.gameObject.GetComponent<Weapon>();
				}
			}
		}
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
		InvokeRepeating("Fire", 0f, primaryWeapon.cooldown);
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
		GameObject projectile = (GameObject)Instantiate (primaryWeapon.projectile, transform.position, transform.rotation);
		float angleDiffLook = AngleDiff(transform.up, worldLookPoint);
		Debug.Log ("Diff is " + angleDiffLook);
		Vector3 angles = projectile.transform.eulerAngles;
		angles.z -= angleDiffLook;
		projectile.transform.eulerAngles = angles;
//		projectile.transform.LookAt(worldLookPoint, Vector3.up);
//		projectile.transform.localEulerAngles
		projectile.transform.parent = projectileGrouping.transform;
	}

	void GetWorldLookPoint () {
		TopdownLook topdownLook = GetComponent<TopdownLook>();
		worldLookPoint = topdownLook.worldLookPoint;
	}

}
