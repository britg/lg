using UnityEngine;
using System.Collections;

public class WeaponLock {
	
	public enum State {
		NotLocked,
		Locking,
		Locked
	}

	public float currentLockingTime = 0f;
	public GameObject currentTarget;
	public State currentLockState = State.NotLocked;

	public bool isLocking {
		get {
			return currentLockState == State.Locking;
		}
	}

	public bool isLocked {
		get {
			return currentLockState == State.Locked;
		}
	}

	public bool isNotLocked {
		get {
			return currentLockState == State.NotLocked;
		}
	}

	public bool isActive {
		get {
			return isLocking || isLocked;
		}
	}

	public bool ValidTarget (GameObject target) {
		return target != null && target.GetComponent<WeaponTarget>() != null;
	}

	public void StartLocking (GameObject target) {
		currentTarget = target;
		currentLockState = State.Locking;
		currentLockingTime = 0f;
	}

	public void ContinueLocking (float delta) {
		currentLockingTime += delta;
	}

	public void Locked () {
		currentLockState = State.Locked;
	}

	public void Break () {
		currentLockState = State.NotLocked;
		currentLockingTime = 0f;
		currentTarget = null;
	}

}