using UnityEngine;
using System.Collections;

public class PlayerDefaults {

	public static Vector3 position = Vector3.zero;
	public static Vector3 rotation = Vector3.zero;
	public static Vector3 scale = new Vector3(1f, 1f, 1f);

	public static Hashtable stats  = new Hashtable {
		{ LG.s_shields, 0.0f },
		{ LG.s_hull, 100f },
		{ LG.s_speed, 25f },
		{ LG.s_fuel, 100f },
		{ LG.s_fuelBurn, 1f }
	};

	public static Hashtable resources  = new Hashtable {
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

		formData.AddField("player[x]", position.x.ToString());
		formData.AddField("player[y]", position.y.ToString());
		formData.AddField("player[z]", position.z.ToString());

		formData.AddField("player[rotation][x]", rotation.x.ToString());
		formData.AddField("player[rotation][y]", rotation.y.ToString());
		formData.AddField("player[rotation][z]", rotation.z.ToString());

		formData.AddField("player[scale][x]", scale.x.ToString());
		formData.AddField("player[scale][y]", scale.y.ToString());
		formData.AddField("player[scale][z]", scale.z.ToString());

		foreach (DictionaryEntry pair in stats) {
			string k = "player[stats][" + pair.Key.ToString() + "]";
			string v = pair.Value.ToString();
			formData.AddField(k, v);
		}
		foreach (DictionaryEntry pair in resources) {
			string k = "player[resources][" + pair.Key.ToString() + "]";
			string v = pair.Value.ToString();
			formData.AddField(k, v);
		}

		return formData;
	}
}
