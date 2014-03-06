using UnityEngine;
using System.Collections;

public class ProcessorBehaviour : LGMonoBehaviour {

	public GameObject AimTarget (Vector3 direction, float range) {
		return AimTarget (direction * range);
	}

	public GameObject AimTarget (Vector3 range) {
		GameObject target = null;
		RaycastHit hit;
		if (Physics.Raycast(transform.position, range.normalized, out hit, range.magnitude)) {
			target = hit.collider.gameObject;
		}
		return target;
	}
}
