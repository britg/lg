﻿using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public Transform target;

	public float life = 2.0f;
	public float velocity = 1000.0f;
	public Transform impactEffect;
	public Transform explosionEffect;
	public Transform firedBy {get; set;}

	// Private variables
	private Vector3 _velocity;
	private Vector3 _newPos;
	private Vector3 _oldPos;	

	void Start () {
		// Set the new position to the current position of the transform
		_newPos = transform.position;
		// Set the old position to the same value
		_oldPos = _newPos;			
		// Set the velocity vector 3 to the specified velocity and set the direction to face forward of the transform
		_velocity = velocity * transform.forward;
		// Set the gameobject to destroy after period "life"
		Destroy(gameObject, life);
	}

	void SetTarget (Transform _target) {
		target = _target;
	}
	
	void Update () {
		Vector3 dir = (target.position - transform.position).normalized;
		_newPos += dir * _velocity.magnitude * Time.deltaTime;
		// SDet direction to the difference between new position and old position
		Vector3 _direction = _newPos - _oldPos;
		// Get the distance which is the magnitude of the direction vector
		float _distance = _direction.magnitude;
		
		// If distance is greater than nothing...
		if (_distance > 0) {
			// Define a RayCast
			RaycastHit _hit;
			// If the raycast from previous position in the specified direction at (or before) the distance...
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
		// Set the old position tho the current position
		_oldPos = transform.position;
		// Set the actual position to the new position
		transform.position = _newPos;		
	}
}
