﻿using UnityEngine;
using System;
using System.Collections;
using uLink;

public class Mob : WorldObject {

	public bool alive = true;
	public float removeTime = 30f;

	void Start () {
		MobEditor mobEditor = GetComponent<MobEditor>();
		stats = new StatCollection(mobEditor.statHash());
		resources = new ResourceCollection(mobEditor.resourceHash());
	}

	public override void TakeDamage (float amount) {
		if (!alive) {
			return;
		}

		float currentShields = stats.Get(Stat.shields).value;
		float diff = 0f;

		if (currentShields >= amount) {
			stats.Remove(Stat.shields, amount);
		}

		if (currentShields < amount) {
			diff = amount - currentShields;
			stats.Set(Stat.shields, 0f);
			stats.Remove(Stat.hull, diff);
		}

		networkView.UnreliableRPC(MobClient.Client_TakeDamage, uLink.RPCMode.Others, amount);

		CheckCurrentState();
	}

	void CheckCurrentState () {
		if (stats.Hull <= 0f) {
			Die();
		}
	}

	public void Die () {
		alive = false;

		WeaponTarget weaponTarget = GetComponent<WeaponTarget>();
		Destroy (weaponTarget);

		networkView.UnreliableRPC(MobClient.Client_Die, uLink.RPCMode.Others);
		Invoke (Server_Remove, removeTime);
	}

	void Extract (ExtractorProcessor processor) {
		Resource[] extraction = new Resource[2];
		extraction[0] = new Resource(Resource.tier1, 2);
		extraction[1] = new Resource(Resource.tier2, 1);
		processor.YieldResources(extraction);
	}

	public static string Server_Remove = "Remove";
	public void Remove () {
		uLink.Network.Destroy(gameObject);
	}

}
