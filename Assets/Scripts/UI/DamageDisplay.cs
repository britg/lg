using UnityEngine;
using System.Collections;

public class DamageDisplay : DisplayBehaviour {

	public static string Client_Display = "DisplayDamage";
	[RPC]
	void DisplayDamage (float amount) {
		Debug.Log ("Displaying damage " + amount);
		GameObject damageDisplay = NGUITools.AddChild(labels, damageLabelPrefab);
		UIFollowTarget follow = GetFollow(damageDisplay);
		follow.target = transform;
		HUDText hudText = GetHUDText(damageDisplay);
		hudText.offsetCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.3f, 40f) });
		hudText.Add(-amount, Color.red, 0.3f);
	}
}
