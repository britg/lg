using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	private Transform _target;
	public Transform target {
		get {
			return _target;
		}
		set {
			_target = value;
		}
	}
	public Vector3 direction = Vector3.zero;

	[HideInInspector]
	public float life;
	[HideInInspector]
	public float velocity;
	[HideInInspector]
	public bool destroyOnImpact;
	[HideInInspector]
	public ProjectileType type;

	public Transform impactEffect;
	public Transform explosionEffect;

	// Private variables
	Vector3 _velocity;
	Vector3 _newPos;
	Vector3 _oldPos;	

	void Start () {
		_newPos = transform.position;
		_oldPos = _newPos;			
		_velocity = velocity * transform.forward;

		if (direction.Equals(Vector3.zero)) {
			direction = (target.position - transform.position).normalized;
		}

		Destroy(gameObject, life);
	}

	void Update () {
		_newPos += direction * _velocity.magnitude * Time.deltaTime;

		if (destroyOnImpact) {
			DetectImpact();
		}

		transform.position = _newPos;		

		// TODO update direction if seeking
	}

	void DetectImpact () {
		Vector3 _direction = _newPos - _oldPos;
		float _distance = _direction.magnitude;

		if (_distance > 0) {
			RaycastHit _hit;
			if (Physics.Raycast(_oldPos, _direction, out _hit, _distance)) {
				if (_hit.transform == target) {
//					 Set the rotation of the impact effect to the normal of the impact surface (we wan't the impact effect to
//					 throw particles out from the object we just hit...
					Quaternion _rotation = Quaternion.FromToRotation(Vector3.up, _hit.normal);

					if (uLink.Network.isClient) {
						Instantiate(impactEffect, _hit.point, _rotation);
					}

					Destroy(gameObject);
				}
			}
		}

		_oldPos = transform.position;
	}

}
