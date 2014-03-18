using UnityEngine;
using System.Collections;

public class MobEditor : MonoBehaviour {

	public int worldObjectId = -1;
	public string worldObjectName = "Mob";
	public bool networked = true;

	public float shields;
	public float hull;
	public float speed;
	public float ammo;
	public float fuel;
	public float fuelBurn;

	public int tier1;
	public int tier2;
	public int tier3;
	public int tier4;
	public int tier5;
	public int tier6;
	public int tier7;

	public Hashtable statHash () {
		return iTween.Hash(
			Stat.shields, shields,
			Stat.hull, hull,
			Stat.speed, speed,
			Stat.ammo, ammo,
			Stat.fuel, fuel,
			Stat.fuelBurn, fuelBurn
			);
	}

	public Hashtable resourceHash () {
		return iTween.Hash (
			Resource.tier1, tier1,
			Resource.tier2, tier2,
			Resource.tier3, tier3,
			Resource.tier4, tier4,
			Resource.tier5, tier5,
			Resource.tier6, tier6,
			Resource.tier7, tier7
			);
	}
	
	public WWWForm toFormData (string apiResource) {
		WWWForm formData = new WWWForm();

		formData.AddField(apiResource + "[name]", worldObjectName);
		formData.AddField(apiResource + "[networked]", networked ? "1" : "0");

		formData.AddField(apiResource + "[x]", transform.position.x.ToString());
		formData.AddField(apiResource + "[y]", transform.position.y.ToString());
		formData.AddField(apiResource + "[z]", transform.position.z.ToString());
		
		formData.AddField(apiResource + "[rotation][x]", transform.eulerAngles.x.ToString());
		formData.AddField(apiResource + "[rotation][y]", transform.eulerAngles.y.ToString());
		formData.AddField(apiResource + "[rotation][z]", transform.eulerAngles.z.ToString());
		
		formData.AddField(apiResource + "[scale][x]", transform.localScale.x.ToString());
		formData.AddField(apiResource + "[scale][y]", transform.localScale.y.ToString());
		formData.AddField(apiResource + "[scale][z]", transform.localScale.z.ToString());
		
		formData.AddField(apiResource + "[stats][" + Stat.shields + "]", shields.ToString());
		formData.AddField(apiResource + "[stats][" + Stat.hull + "]", hull.ToString());
		formData.AddField(apiResource + "[stats][" + Stat.speed + "]", speed.ToString());
		formData.AddField(apiResource + "[stats][" + Stat.ammo + "]", ammo.ToString());
		formData.AddField(apiResource + "[stats][" + Stat.fuel + "]", fuel.ToString());
		formData.AddField(apiResource + "[stats][" + Stat.fuelBurn + "]", fuelBurn.ToString());

		formData.AddField(apiResource + "[resources][" + Resource.tier1 + "]", tier1.ToString());
		formData.AddField(apiResource + "[resources][" + Resource.tier2 + "]", tier2.ToString());
		formData.AddField(apiResource + "[resources][" + Resource.tier3 + "]", tier3.ToString());
		formData.AddField(apiResource + "[resources][" + Resource.tier4 + "]", tier4.ToString());
		formData.AddField(apiResource + "[resources][" + Resource.tier5 + "]", tier5.ToString());
		formData.AddField(apiResource + "[resources][" + Resource.tier6 + "]", tier6.ToString());
		formData.AddField(apiResource + "[resources][" + Resource.tier7 + "]", tier7.ToString());
		
		return formData;
	}
}
