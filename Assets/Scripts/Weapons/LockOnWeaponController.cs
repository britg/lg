using UnityEngine;
using System.Collections;

public class LockOnWeaponController : WeaponController {

	public float lockCheckInterval = 0.1f;

	WeaponLock weaponLock = new WeaponLock();
	WeaponLockDisplay weaponLockDisplay;

	public int lockPercentage {
		get {
			return Mathf.Clamp(Mathf.RoundToInt((weaponLock.currentLockingTime / player.stat (Stat.weaponTargetTime))*100), 0, 100);
		}
	}

	protected override void ControllerStart () {
		weaponLockDisplay = gameObject.AddComponent<WeaponLockDisplay>();
		NotificationCenter.AddObserver(this, LG.n_playerStatsLoaded);
		NotificationCenter.AddObserver(this, LG.n_registerWeapon);
	}

	protected override void ControllerUpdate () {
		if (weaponLock.isLocking) {
			weaponLock.ContinueLocking(Time.deltaTime);
		}
	}

	protected override void DetectInput () {

		if (Input.GetMouseButtonUp(0)) {
			BreakLock();
		}

		if (Input.GetMouseButton(0) && !weaponLock.isActive) {
			StartLockAttempt();
		}

	}

	void StartLockAttempt () {
		GameObject currentTarget = AimTarget(player.stat(Stat.weaponRange));
		if (weaponLock.ValidTarget(currentTarget)) {
			Debug.Log ("Starting lock attempt");
			networkView.UnreliableRPC(LockOnWeaponProcessor.Server_StartTargetLock, uLink.RPCMode.Server, currentLookDirection);
			weaponLock.StartLocking(currentTarget);
			InvokeRepeating("ContinueLockAttempt", lockCheckInterval, lockCheckInterval);
			weaponLockDisplay.StartDisplay(weaponLock.currentTarget);
		}
	}

	void ContinueLockAttempt () {
		networkView.UnreliableRPC(LockOnWeaponProcessor.Server_CheckTargetLock, uLink.RPCMode.Server, currentLookDirection);
		weaponLockDisplay.UpdateDisplay(lockPercentage);
	}

	public static string Client_BreakLock = "BreakLock";
	[RPC] void BreakLock () {
		Debug.Log ("Breaking lock");
		weaponLock.Break();
		CancelInvoke();
		weaponLockDisplay.BreakDisplay();
	}

	public static string Client_CompleteLock = "CompleteLock";
	[RPC] void CompleteLock () {
		Debug.Log ("Lock completed");
		weaponLock.Locked();
		weaponLockDisplay.CompleteDisplay();
	}

	public static string Client_TriggerWeaponDisplay = "TriggerWeaponDisplay";
	[RPC] void TriggerWeaponDisplay () {
		Debug.Log ("Triggering weapon display");
		if (weaponLock.currentTarget != null) {
			weapon.Fire(weaponLock.currentTarget.transform);
		}
	}
	
}
