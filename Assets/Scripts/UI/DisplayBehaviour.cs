﻿using UnityEngine;
using System.Collections;

public class DisplayBehaviour : LGMonoBehaviour {

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

	Labeler _labeler;
	public Labeler labeler {
		get {
			if (_labeler == null) {
				_labeler = labels.GetComponent<Labeler>();
			}
			return _labeler;
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
