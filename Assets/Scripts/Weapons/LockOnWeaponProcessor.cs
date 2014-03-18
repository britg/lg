using UnityEngine;
using System.Collections;

public class LockOnWeaponProcessor : WeaponProcessor {

	public float timeToHit = 0.2f;
	WeaponLock weaponLock = new WeaponLock();

	GameObject thisTarget;

	void Update () {
		if (weaponLock.isLocking) {
			weaponLock.ContinueLocking(Time.deltaTime);
		}
	}

	public const string Server_StartTargetLock = "StartTargetLock";
	[RPC]
	void StartTargetLock (Vector3 direction) {
//		Debug.Log ("Starting weapon lock state");
		GameObject currentTarget = AimTarget(direction, playerProcessor.stat(Stat.weaponRange));
		if (weaponLock.ValidTarget(currentTarget)) {
			weaponLock.StartLocking(currentTarget);
		} else {
			BreakLock();
		}
	}

	public const string Server_CheckTargetLock = "CheckTargetLock";
	[RPC]
	void CheckTargetLock (Vector3 direction) {
//		Debug.Log ("Checking weapon lock " + direction);

		if (weaponLock.currentLockingTime >= playerProcessor.stat(Stat.weaponTargetTime)) {
			LockSuccess();
			return;
		}

		GameObject currentTarget = AimTarget(direction, playerProcessor.stat(Stat.weaponRange));
		if (currentTarget != null) {
			if (currentTarget != weaponLock.currentTarget) {
				BreakLock();
			}

			Mob mob = currentTarget.GetComponent<Mob>();
			if (mob != null && !mob.alive) {
				BreakLock();
			}
		} else {
			BreakLock();
		}

	}

	void LockSuccess () {
		Debug.Log ("Lock success from server");
		weaponLock.Locked();
		networkView.UnreliableRPC(LockOnWeaponController.Client_CompleteLock, uLink.RPCMode.Owner);
	}

	void BreakLock () {
		if (weaponLock.isActive) {
			Debug.Log ("Breaking server lock");
			weaponLock.Break();
			networkView.UnreliableRPC(LockOnWeaponController.Client_BreakLock, uLink.RPCMode.Owner);
		}
	}

	public const string Server_TriggerWeapon = "TriggerWeapon";
	[RPC]
	void TriggerWeapon (Vector3 direction) {
		thisTarget = AimTarget(direction, playerProcessor.stat (Stat.weaponRange));
		if (weaponLock.ValidTarget(thisTarget)) {
			networkView.UnreliableRPC(LockOnWeaponController.Client_TriggerWeaponDisplay, uLink.RPCMode.Owner);
			Invoke ("DoDamage", timeToHit);
		} else {
			BreakLock();
		}
	}

	void DoDamage () {
		float amount = playerProcessor.stat(Stat.weaponDamage);
		thisTarget.SendMessage(WorldObject.Server_TakeDamage, amount);
		BreakLock();
	}
}
