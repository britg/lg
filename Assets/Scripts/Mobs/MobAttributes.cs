using UnityEngine;
using System;
using System.Collections;

public class MobAttributes : LGMonoBehaviour {

	public int persistId;

	void Awake () {
		transform.parent = GameObject.Find ("Mobs").transform;
	}

	[Serializable]
	public class ShipAttributes {
		
		public string name = "Starter Mob Ship";
		
		public int 		shields = 0;
		public int 		hull = 100;
		public float 	speed = 6f;
		
	}
	
	[Serializable]
	public class WeaponAttributes {
		
		public float 	cooldown = 0.2f;
		public Vector3 	aim = Vector3.zero;
		public float 	velocity = 500f;
		public float 	life = 2f;
		public float 	damage = 10f;
		
	}
	
	public ShipAttributes shipAttributes = new ShipAttributes();
	public WeaponAttributes weaponAttributes = new WeaponAttributes();

	public void SyncAttributes (string rawAttributes) {
		Debug.Log ("Syncing mob attributes from server to client");
		networkView.RPC ("AssignAttributes", uLink.RPCMode.All, rawAttributes);
	}
	
	[RPC]
	public void AssignAttributes (string rawAttributes) {
		Debug.Log ("Assigning attributes to this mob " + rawAttributes);
		Hashtable attributes = MiniJSON.Json.Hashtable(rawAttributes);

		IDictionary props = (IDictionary) attributes["properties"];

		int.TryParse(attributes["id"].ToString (), out persistId);
		int.TryParse(props["health"].ToString(), out shipAttributes.hull);
	}
}
