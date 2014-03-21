using UnityEngine;
using System.Collections;

public class LockOnWeaponProcessor : WeaponProcessor {

	WeaponLock weaponLock = new WeaponLock();

	GameObject thisTarget;
	Vector3 currentDirection;

	void Update () {
		if (weaponLock.isActive) {
			weaponLock.ContinueLocking(Time.deltaTime);
		}
	}

	public const string Server_StartTargetLock = "StartTargetLock";
	[RPC] void StartTargetLock (Vector3 direction) {
		GameObject currentTarget = AimTarget(direction, playerProcessor.stat(Stat.weaponRange));
		if (weaponLock.ValidTarget(currentTarget)) {
			weaponLock.StartLocking(currentTarget);
		} else {
			BreakLock();
		}
	}

	public const string Server_CheckTargetLock = "CheckTargetLock";
	[RPC] void CheckTargetLock (Vector3 direction) {
		currentDirection = direction;
		if (weaponLock.currentLockingTime >= playerProcessor.stat(Stat.weaponTargetTime)) {
			if (weaponLock.isLocking)  {
				LockSuccess();
			}
			return;
		}

		GameObject currentTarget = AimTarget(currentDirection, playerProcessor.stat(Stat.weaponRange));
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
		TriggerWeapon();
		InvokeRepeating (Server_TriggerWeapon, weapon.volleyDelay, weapon.volleyDelay);
	}

	void BreakLock () {
		CancelInvoke();
		if (weaponLock.isActive) {
			Debug.Log ("Breaking server lock");
			weaponLock.Break();
			networkView.UnreliableRPC(LockOnWeaponController.Client_BreakLock, uLink.RPCMode.Owner);
		}
	}

	public const string Server_TriggerWeapon = "TriggerWeapon";
	void TriggerWeapon () {
		thisTarget = AimTarget(currentDirection, playerProcessor.stat (Stat.weaponRange));
		if (weaponLock.ValidTarget(thisTarget)) {
			networkView.UnreliableRPC(LockOnWeaponController.Client_TriggerWeaponDisplay, uLink.RPCMode.Owner);
			DoDamage();
		} else {
			BreakLock();
		}
	}

	void DoDamage () {
		float amount = playerProcessor.stat(Stat.weaponDamage);
		thisTarget.SendMessage(WorldObject.Server_TakeDamage, amount);
	}
}
