using UnityEngine;
using System;
using System.Collections;

public class Mob : WorldObject {

	[Serializable]
	public class MobShipAttributes {
		
		public string name = "Starter Mob Ship";
		
		public int 		shields = 0;
		public int 		hull = 100;
		public float 	speed = 6f;
		
	}
	
	[Serializable]
	public class MobWeaponAttributes {
		
		public float 	cooldown = 0.2f;
		public Vector3 	aim = Vector3.zero;
		public float 	velocity = 500f;
		public float 	life = 2f;
		public float 	damage = 10f;
		
	}
	
	public MobShipAttributes shipAttributes = new MobShipAttributes();
	public MobWeaponAttributes weaponAttributes = new MobWeaponAttributes();
}
