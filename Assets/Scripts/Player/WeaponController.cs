using UnityEngine;
using System.Collections;

public class WeaponController : ControllerBehaviour {

	public static string Client_BreakLock = "BreakLock";
	public static string Client_CompleteLock = "CompleteLock";

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
			networkView.UnreliableRPC(WeaponProcessor.Server_StartTargetLock, uLink.RPCMode.Server, currentLookDirection);
			weaponLock.StartLocking(currentTarget);
			InvokeRepeating("ContinueLockAttempt", lockCheckInterval, lockCheckInterval);
			weaponLockDisplay.StartDisplay(weaponLock.currentTarget);
		}
	}

	void ContinueLockAttempt () {
//		Debug.Log ("Continuing lock attempt");
		networkView.UnreliableRPC(WeaponProcessor.Server_CheckTargetLock, uLink.RPCMode.Server, currentLookDirection);
		weaponLockDisplay.UpdateDisplay(lockPercentage);
	}

	[RPC]
	void BreakLock () {
		Debug.Log ("Breaking lock");
		weaponLock.Break();
		CancelInvoke();
		weaponLockDisplay.BreakDisplay();
	}

	[RPC]
	void CompleteLock () {
		Debug.Log ("Lock completed");
		weaponLock.Locked();
		CancelInvoke();
		weaponLockDisplay.CompleteDisplay();
	}

	void TriggerWeapon () {
		networkView.UnreliableRPC(WeaponProcessor.Server_TriggerWeapon, uLink.RPCMode.Server, currentLookDirection);
	}

	public static string Client_TriggerWeaponDisplay = "TriggerWeaponDisplay";
	[RPC]
	void TriggerWeaponDisplay () {
		Debug.Log ("Triggering weapon display");
		weapon.SendMessage("Fire", weaponLock.currentTarget.transform);
	}
	
}
