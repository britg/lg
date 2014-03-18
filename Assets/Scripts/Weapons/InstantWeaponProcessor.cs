using UnityEngine;
using System.Collections;

public class InstantWeaponProcessor : WeaponProcessor {

	// Use this for initialization
	void Start () {
	
	}

	void Update () {

	}

	public static string Server_TriggerWeapon = "TriggerWeapon";
	[RPC] void TriggerWeapon (Vector3 direction) {
		// Raycast hit anything?
		RaycastHit hit;
		if (Physics.Raycast(transform.position, direction, out hit, weapon.Range)) {
			GameObject thisTarget = hit.collider.gameObject;
			float amount = playerProcessor.stat(Stat.weaponDamage);
			thisTarget.SendMessage(WorldObject.Server_TakeDamage, amount);
		}
	}
	
}
