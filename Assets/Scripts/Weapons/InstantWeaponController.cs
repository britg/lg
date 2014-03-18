using UnityEngine;
using System.Collections;

public class InstantWeaponController : WeaponController {

	// Use this for initialization
	protected override void ControllerStart () {
	
	}

	protected override void DetectInput () {
		if (Input.GetMouseButtonDown(0)) {
			TriggerWeapon();
		}
	}

	void TriggerWeapon () {
		weapon.FireTowards(currentLookDirection);
		networkView.UnreliableRPC(InstantWeaponProcessor.Server_TriggerWeapon, uLink.RPCMode.Server, currentLookDirection);

	}
}
