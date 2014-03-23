using UnityEngine;
using System.Collections;

// Refactory to Label Display
public class LabelsDisplay : DisplayBehaviour {

	public GameObject objectLabel;
	public GameObject weaponLockLabel;

	// Use this for initialization
	void Start () {
		NotificationCenter.AddObserver(this, LG.n_AddLabel);
	}

	void OnAddLabel (Notification n) {
		GameObject labelObj = NGUITools.AddChild(uiRoot, objectLabel);
		UILabel label = labelObj.GetComponent<UILabel>();
		label.text = (string)n.data["label"];
		UIFollowTarget followTarget = labelObj.GetComponent<UIFollowTarget>();
		followTarget.target = transform;
	}
}
