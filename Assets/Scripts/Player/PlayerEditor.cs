using UnityEngine;
using System.Collections;

public class PlayerEditor : MonoBehaviour {

	public string templateName = "_template";

	public Vector3 startPosition;
	public Vector3 startRotation;
	public Vector3 startScale;

	public float shields;
	public float hull;
	public float speed;

	public float ammo;
	public float weaponRange;
	public float weaponTargetTime;
	public float weaponLoseTargetTime;

	public float fuel;
	public float fuelBurn;

	public float extractorRate;
	public float extractorStrength;
	public float extractorLength;

	public int tier1;
	public int tier2;
	public int tier3;
	public int tier4;
	public int tier5;
	public int tier6;
	public int tier7;

	public WWWForm toFormData () {
		WWWForm formData = new WWWForm();
		
		formData.AddField("player[x]", startPosition.x.ToString());
		formData.AddField("player[y]", startPosition.y.ToString());
		formData.AddField("player[z]", startPosition.z.ToString());
		
		formData.AddField("player[rotation][x]", startRotation.x.ToString());
		formData.AddField("player[rotation][y]", startRotation.y.ToString());
		formData.AddField("player[rotation][z]", startRotation.z.ToString());
		
		formData.AddField("player[scale][x]", startScale.x.ToString());
		formData.AddField("player[scale][y]", startScale.y.ToString());
		formData.AddField("player[scale][z]", startScale.z.ToString());

		formData.AddField("player[stats][" + Stat.shields + "]", shields.ToString());
		formData.AddField("player[stats][" + Stat.hull + "]", hull.ToString());
		formData.AddField("player[stats][" + Stat.speed + "]", speed.ToString());

		formData.AddField("player[stats][" + Stat.ammo + "]", ammo.ToString());
		formData.AddField("player[stats][" + Stat.weaponRange + "]", weaponRange.ToString());
		formData.AddField("player[stats][" + Stat.weaponTargetTime + "]", weaponTargetTime.ToString());
		formData.AddField("player[stats][" + Stat.weaponLoseTargetTime + "]", weaponLoseTargetTime.ToString());

		formData.AddField("player[stats][" + Stat.fuel + "]", fuel.ToString());
		formData.AddField("player[stats][" + Stat.fuelBurn + "]", fuelBurn.ToString());

		formData.AddField("player[stats][" + Stat.extractorRate + "]", extractorRate.ToString());
		formData.AddField("player[stats][" + Stat.extractorStrength + "]", extractorStrength.ToString());
		formData.AddField("player[stats][" + Stat.extractorLength + "]", extractorLength.ToString());

		formData.AddField("player[resources][" + Resource.tier1 + "]", tier1.ToString());
		formData.AddField("player[resources][" + Resource.tier2 + "]", tier2.ToString());
		formData.AddField("player[resources][" + Resource.tier3 + "]", tier3.ToString());
		formData.AddField("player[resources][" + Resource.tier4 + "]", tier4.ToString());
		formData.AddField("player[resources][" + Resource.tier5 + "]", tier5.ToString());
		formData.AddField("player[resources][" + Resource.tier6 + "]", tier6.ToString());
		formData.AddField("player[resources][" + Resource.tier7 + "]", tier7.ToString());
		
		return formData;
	}
}
