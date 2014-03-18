using UnityEngine;
using System.Collections;

public class DamageDisplay : DisplayBehaviour {

	GameObject damageDisplay;
	HUDText damageText;

	void Start () {
	}

	void Update () {
	}


	public void DisplayDamage (float amount) {

		if (damageText == null) {
			damageDisplay = NGUITools.AddChild(labels, damageLabelPrefab);
			UIFollowTarget follow = GetFollow(damageDisplay);
			follow.target = transform;
			damageText = GetHUDText(damageDisplay);
		}

		damageText.offsetCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.2f, 60f) });
		damageText.Add(-amount, Color.red, 0.2f);

		iTween.PunchRotation(gameObject, Vector3.right*10, 0.5f);
		iTween.PunchPosition(gameObject, Vector3.right*10, 0.5f);
	}

	public void DisplayDeath () {
		damageText.Add("Dead!", Color.red, 1f);
		float x = (float)Random.Range (0, 360);
		float y = (float)Random.Range (0, 360);
		float z = (float)Random.Range (0, 360);
		iTween.RotateTo(gameObject, new Vector3(x, y, z), 30f);
	}

	void OnDestroy () {
		Destroy(damageDisplay);
	}

}
