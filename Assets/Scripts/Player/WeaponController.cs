using UnityEngine;
using System.Collections;

public class WeaponController : ControllerBehaviour {

	public static string Client_BreakLock = "BreakLock";
	public static string Client_CompleteLock = "CompleteLock";

	bool weaponsActive = false;
	public float lockCheckInterval = 0.1f;
	WeaponLock weaponLock = new WeaponLock();

	GameObject weaponLockLabelObj;
	UILabel weaponLockLabel;
	UIFollowTarget weaponLockLabelFollow;

	public int lockPercentage {
		get {
			return Mathf.RoundToInt((weaponLock.currentLockingTime / player.stat (Stat.weaponTargetTime))*100);
		}
	}

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
			StartLockDisplay();
		}
	}

	void ContinueLockAttempt () {
//		Debug.Log ("Continuing lock attempt");
		networkView.UnreliableRPC(WeaponProcessor.Server_CheckTargetLock, uLink.RPCMode.Server, currentLookDirection);
		UpdateLockDisplay();
	}

	[RPC]
	void BreakLock () {
		Debug.Log ("Breaking lock");
		weaponLock.Break();
		CancelInvoke();
		BreakLockDisplay();
	}

	[RPC]
	void CompleteLock () {
		Debug.Log ("Lock completed");
		weaponLock.Locked();
		CancelInvoke();
		CompleteLockDisplay();
	}

	void ConnectLockDisplay () {
		weaponLockLabelObj = GameObject.Find("Labels").GetComponent<Labeler>().weaponLockLabel;
		weaponLockLabel = weaponLockLabelObj.transform.Find("Label").GetComponent<UILabel>();
		weaponLockLabelFollow = weaponLockLabelObj.GetComponent<UIFollowTarget>();
	}

	void StartLockDisplay () {
		if (weaponLockLabelObj == null) {
			ConnectLockDisplay();
		}
		weaponLockLabelFollow.target = weaponLock.currentTarget.transform;
		UpdateLockDisplay();
		weaponLockLabelObj.SetActive(true);
	}

	void UpdateLockDisplay () {
		weaponLockLabel.text = "Locking " + lockPercentage + "%";
	}

	void BreakLockDisplay () {
//		weaponLockLabel.text = "Broken!";
		weaponLockLabelObj.SetActive(false);
	}

	void CompleteLockDisplay () {
		weaponLockLabel.text = "Locked!";
	}

}
