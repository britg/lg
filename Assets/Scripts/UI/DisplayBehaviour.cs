using UnityEngine;
using System.Collections;

public class DisplayBehaviour : LGMonoBehaviour {

	public GameObject uiRoot {
		get {
			return GameObject.Find ("UI Root");
		}
	}

	GameObject _objectLabelPrefab;
	public GameObject objectLabelPrefab {
		get {
			if (_objectLabelPrefab == null) {
				_objectLabelPrefab = (GameObject)Resources.Load("ObjectLabel");
			}
			return _objectLabelPrefab;
		}
	}

	GameObject _damageLabelPrefab;
	public GameObject damageLabelPrefab {
		get {
			if (_damageLabelPrefab == null) {
				_damageLabelPrefab = (GameObject)Resources.Load("DamageLabel");
			}
			return _damageLabelPrefab;
		}
	}

	GameObject _labels;
	public GameObject labels {
		get {
			if (_labels == null) {
				_labels = GameObject.Find("Labels");
			}
			return _labels;
		}
	}

	LabelsDisplay _labelsDisplay;
	public LabelsDisplay labelsDisplay {
		get {
			if (_labelsDisplay == null) {
				_labelsDisplay = labels.GetComponent<LabelsDisplay>();
			}
			return _labelsDisplay;
		}
	}

	public UILabel GetLabel (GameObject obj) {
		return obj.transform.Find("Label").GetComponent<UILabel>();
	}

	public UIFollowTarget GetFollow (GameObject obj) {
		return obj.GetComponent<UIFollowTarget>();
	}

	public HUDText GetHUDText (GameObject obj) {
		return obj.transform.Find ("Label").GetComponent<HUDText>();
	}

}
