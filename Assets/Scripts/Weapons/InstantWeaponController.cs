using UnityEngine;
using System.Collections;

public class InstantWeaponController : WeaponController {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		DetectInput();
	}

	
	void DetectInput () {
		if (Input.GetMouseButtonDown(0)) {
			TriggerWeapon();
		}
	}

	void TriggerWeapon () {
		weapon.FireTowards(currentLookDirection);
		networkView.UnreliableRPC(InstantWeaponProcessor.Server_TriggerWeapon, uLink.RPCMode.Server, currentLookDirection);

	}
}
