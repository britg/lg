﻿using UnityEngine;
using System.Collections;

public class ControllerBehaviour : LGMonoBehaviour {

	LookController _lookController;
	public LookController lookController {
		get {
			if (_lookController == null) {
				_lookController = GetComponent<LookController>();
			}
			return _lookController;
		}
	}

	public Vector3 currentLookDirection {
		get {
			return lookController.lookDirection;
		}
	}


	public GameObject AimTarget (float range) {
		GameObject target = null;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, currentLookDirection, out hit, range)) {
			target = hit.collider.gameObject;
		}
		return target;
	}

	void Start () {
		OffOnMenu();
		ControllerStart();
	}

	protected virtual void ControllerStart () {}

	protected override void InputUpdate () {
		ControllerUpdate();
	}

	protected virtual void ControllerUpdate () {}

	protected void OffOnMenu () {
		NotificationCenter.AddObserver(this, LG.n_showMenu);
		NotificationCenter.AddObserver(this, LG.n_hideMenu);
	}

}
