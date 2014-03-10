using UnityEngine;
using System.Collections;

public class LabelDisplay : DisplayBehaviour {

	public string labelText = "";
	public Color color = Color.white;

	GameObject labelObj;
	[HideInInspector]
	public UILabel label;
	[HideInInspector]
	public UIFollowTarget followTarget;

	void Start () {
		AddLabel();
	}

	void AddLabel () {
		labelObj = NGUITools.AddChild(labels, objectLabelPrefab);
		labelObj.name = labelText;

		label = GetLabel(labelObj);
		label.text = labelText;
		label.color = color;

		followTarget = GetFollow(labelObj);
		followTarget.target = transform;
	}

	void OnDestroy () {
		Destroy (labelObj);
	}

}
