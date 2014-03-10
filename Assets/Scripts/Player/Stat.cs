using UnityEngine;
using System.Collections;

[System.Serializable]
public class Stat {

	/* Known usable stats */
	public static string shields = "shields";
	public static string hull = "hull";
	public static string speed = "speed";

	public static string ammo = "ammo";
	public static string weaponDamage = "weapon_damage";
	public static string weaponRange = "weapon_range";
	public static string weaponTargetTime = "weapon_target_time";
	public static string weaponLoseTargetTime = "weapon_lose_target_time";

	public static string fuel = "fuel";
	public static string fuelBurn = "fuel_burn";

	public static string extractorRate = "extractor_rate";
	public static string extractorStrength = "extractor_strength";
	public static string extractorLength = "extractor_length";

	public string name;
	public float value;

	public Stat (string _name, float _value) {
		name = _name;
		value = _value;
	}

	public Stat (string _name, string _vStr) {
		name = _name;
		float.TryParse(_vStr, out value);
	}

	public Stat (DictionaryEntry keyPair) {
		name = (string)keyPair.Key;
		float.TryParse(keyPair.Value.ToString(), out value);
	}

	public override string ToString () {
		return value.ToString();
	}
}
