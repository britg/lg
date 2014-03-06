using UnityEngine;
using System.Collections;

public class WeaponController : ControllerBehaviour {

	public static string Client_BreakLock = "BreakLock";
	public static string Client_CompleteLock = "CompleteLock";

	bool weaponsActive = false;
	public float lockCheckInterval = 0.1f;
	WeaponLock weaponLock = new WeaponLock();

	void Start () {
		NotificationCenter.AddObserver(this, LG.n_playerStatsLoaded);
	}

	void OnPlayerStatsLoaded () {
		Activate();
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

		if (Input.GetMouseButtonUp(0) && (weaponLock.isLocking || weaponLock.isLocked)) {
			BreakLock();
		}

		if (Input.GetMouseButton(0) && weaponLock.isNotLocked) {
			StartLockAttempt();
		}

		if (Input.GetMouseButton(0) && (weaponLock.isLocking || weaponLock.isLocked)) {

		}
	}

	void StartLockAttempt () {
		GameObject currentTarget = AimTarget(player.stat(Stat.weaponRange));
		if (currentTarget != null) {
			Debug.Log ("Starting lock attempt");
			networkView.UnreliableRPC(WeaponProcessor.Server_StartTargetLock, uLink.RPCMode.Server, currentLookDirection);
			weaponLock.StartLocking(currentTarget);
			InvokeRepeating("ContinueLockAttempt", lockCheckInterval, lockCheckInterval);
		}
	}

	void ContinueLockAttempt () {
		Debug.Log ("Continuing lock attempt");
		networkView.UnreliableRPC(WeaponProcessor.Server_CheckTargetLock, uLink.RPCMode.Server, currentLookDirection);
	}

	[RPC]
	void BreakLock () {
		Debug.Log ("Breaking lock");
		weaponLock.Break();
		CancelInvoke();
	}

	[RPC]
	void CompleteLock () {
		Debug.Log ("Lock completed");
		weaponLock.Locked();
		CancelInvoke();
	}

}
