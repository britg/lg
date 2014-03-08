using UnityEngine;
using System.Collections;

public class HasLabel : MonoBehaviour {

	public string labelText = "";
	public int offsetY = 16;
	public int offsetX = 0;
	public Color color = Color.white;

	[HideInInspector]
	public GameObject ui;
	public GameObject objectLabel;
	[HideInInspector]
	public UILabel label;
	[HideInInspector]
	public UIFollowTarget followTarget;

	void Start () {
		if (ui == null) {
			ui = GameObject.Find ("Labels");
		}
		AddLabel();
	}

	void AddLabel () {
		GameObject labelObj = NGUITools.AddChild(ui, objectLabel);
		label = labelObj.GetComponent<UILabel>();
		label.text = labelText;
		label.color = color;
		label.spacingY = offsetY;
		followTarget = labelObj.GetComponent<UIFollowTarget>();
		followTarget.target = transform;
	}

}
