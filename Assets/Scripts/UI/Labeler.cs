using UnityEngine;
using System.Collections;

public class Labeler : MonoBehaviour {

	public GameObject objectLabel;
	public GameObject weaponLockLabel;

	GameObject ui;

	// Use this for initialization
	void Start () {
		ui = GameObject.Find ("UI Root");
		NotificationCenter.AddObserver(this, LG.n_AddLabel);
	}

	void OnAddLabel (Notification n) {
		GameObject labelObj = NGUITools.AddChild(ui, objectLabel);
		UILabel label = labelObj.GetComponent<UILabel>();
		label.text = (string)n.data["label"];
		UIFollowTarget followTarget = labelObj.GetComponent<UIFollowTarget>();
		followTarget.target = transform;
	}
}
