using UnityEngine;
using System.Collections;

public class ControllerBehaviour : LGMonoBehaviour {

	protected bool acceptInput = true;

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

	void Update () {
		if (acceptInput) {
			DetectInput();
		}
		ControllerUpdate();
	}

	protected virtual void ControllerUpdate () {}
	protected virtual void DetectInput () {}

	protected void OffOnMenu () {
		NotificationCenter.AddObserver(this, LG.n_showMenu);
		NotificationCenter.AddObserver(this, LG.n_hideMenu);
	}

	void OnShowMenu () {
		acceptInput = false;
	}

	void OnHideMenu () {
		acceptInput = true;
	}

}
