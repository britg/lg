using UnityEngine;
using System.Collections;

public class WeaponProcessor : ProcessorBehaviour {

	public const string Server_StartTargetLock = "StartTargetLock";
	public const string Server_CheckTargetLock = "CheckTargetLock";

	WeaponLock weaponLock = new WeaponLock();

	void Update () {
		if (weaponLock.isLocking) {
			weaponLock.ContinueLocking(Time.deltaTime);
		}
	}

	[RPC]
	void StartTargetLock (Vector3 direction) {
		Debug.Log ("Starting weapon lock state");
		GameObject currentTarget = AimTarget(direction, playerProcessor.stat(Stat.weaponRange));
		if (currentTarget != null) {
			weaponLock.StartLocking(currentTarget);
		} else {
			BreakLock();
		}
	}

	[RPC]
	void CheckTargetLock (Vector3 direction) {
		Debug.Log ("Checking weapon lock " + direction);

		if (weaponLock.currentLockingTime >= playerProcessor.stat(Stat.weaponTargetTime)) {
			LockSuccess();
			return;
		}

		GameObject currentTarget = AimTarget(direction, playerProcessor.stat(Stat.weaponRange));
		if (currentTarget != weaponLock.currentTarget) {
			// TEMP: We really want to have a buffer here.
			BreakLock ();
		}
	}

	void LockSuccess () {
		Debug.Log ("Lock success from server");
		weaponLock.Locked();
		networkView.UnreliableRPC(WeaponController.Client_CompleteLock, uLink.RPCMode.Owner);
	}

	void BreakLock () {
		Debug.Log ("Breaking server lock");
		weaponLock.Break();
		networkView.UnreliableRPC(WeaponController.Client_BreakLock, uLink.RPCMode.Owner);
	}
}
