using UnityEngine;
using System.Collections;

public class LockOnWeaponController : ControllerBehaviour {

	public float lockCheckInterval = 0.1f;

	bool weaponsActive = false;
	WeaponLock weaponLock = new WeaponLock();
	WeaponLockDisplay weaponLockDisplay;
	GameObject weapon;

	public int lockPercentage {
		get {
			return Mathf.Clamp(Mathf.RoundToInt((weaponLock.currentLockingTime / player.stat (Stat.weaponTargetTime))*100), 0, 100);
		}
	}

	void Start () {
		weaponLockDisplay = gameObject.AddComponent<WeaponLockDisplay>();
		NotificationCenter.AddObserver(this, LG.n_playerStatsLoaded);
		NotificationCenter.AddObserver(this, LG.n_registerWeapon);
	}

	void OnPlayerStatsLoaded () {
		Activate();
	}

	void OnRegisterWeapon (Notification n) {
		weapon = n.data["weapon"] as GameObject;
	}

	void Activate () {
		weaponsActive = true;
	}

	void Update () {
		if (weaponsActive) {
			DetectInput();
		}

		if (weaponLock.isLocking) {
			weaponLock.ContinueLocking(Time.deltaTime);
		}
	}

	void DetectInput () {

		if (Input.GetMouseButtonUp(0) && (weaponLock.isLocking)) {
			BreakLock();
		}

		if (Input.GetMouseButtonUp(0) && weaponLock.isLocked) {
			TriggerWeapon();
		}

		if (Input.GetMouseButton(0) && weaponLock.isNotLocked) {
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
//		Debug.Log ("Continuing lock attempt");
		networkView.UnreliableRPC(LockOnWeaponProcessor.Server_CheckTargetLock, uLink.RPCMode.Server, currentLookDirection);
		weaponLockDisplay.UpdateDisplay(lockPercentage);
	}

	public static string Client_BreakLock = "BreakLock";
	[RPC]
	void BreakLock () {
		Debug.Log ("Breaking lock");
		weaponLock.Break();
		CancelInvoke();
		weaponLockDisplay.BreakDisplay();
	}

	public static string Client_CompleteLock = "CompleteLock";
	[RPC]
	void CompleteLock () {
		Debug.Log ("Lock completed");
		weaponLock.Locked();
		CancelInvoke();
		weaponLockDisplay.CompleteDisplay();
		TriggerWeapon();
	}

	void TriggerWeapon () {
		networkView.UnreliableRPC(LockOnWeaponProcessor.Server_TriggerWeapon, uLink.RPCMode.Server, currentLookDirection);
	}

	public static string Client_TriggerWeaponDisplay = "TriggerWeaponDisplay";
	[RPC]
	void TriggerWeaponDisplay () {
		Debug.Log ("Triggering weapon display");
		if (weaponLock.currentTarget != null) {
			weapon.SendMessage("Fire", weaponLock.currentTarget.transform);
		}
	}
	
}
