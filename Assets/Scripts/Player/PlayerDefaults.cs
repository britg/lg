using UnityEngine;
using System.Collections;

public class PlayerDefaults {

	public static Hashtable defaults = new Hashtable {

		{ "x", 0.0f },
		{ "y", 0.0f },
		{ "z", 0.0f },

		{ LG.s_shields, 0.0f },
		{ LG.s_hull, 100f },
		{ LG.s_speed, 25f },
		{ LG.s_fuel, 100f },
		{ LG.s_fuelBurn, 1f },

		{ LG.r1, 0f },
		{ LG.r2, 0f },
		{ LG.r3, 0f },
		{ LG.r4, 0f },
		{ LG.r5, 0f },
		{ LG.r6, 0f },
		{ LG.r7, 0f }

	};

	public static WWWForm toFormData () {
		WWWForm formData = new WWWForm();
		foreach (DictionaryEntry pair in defaults) {
			string k = "player[properties][" + pair.Key.ToString() + "]";
			string v = pair.Value.ToString();
			formData.AddField(k, v);
		}

		return formData;
	}
}
