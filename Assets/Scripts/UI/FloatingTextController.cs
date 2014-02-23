using UnityEngine;
using System.Collections;

public class FloatingTextController : MonoBehaviour {

	public GameObject labelPrefab;

	float scale;

	void Start () {
		scale = GameObject.Find ("UI Root").transform.localScale.x;
	}

	public void Notify (GameObject onObject, string text) {
		GameObject label = (GameObject)Instantiate(labelPrefab, Vector3.zero, Quaternion.identity);
		label.transform.parent = transform;
		label.transform.localPosition = Vector3.zero;
		label.transform.localScale = new Vector3(1f, 1f, 1f);
		UILabel labelLabel = label.GetComponent<UILabel>();
		labelLabel.text = text;
		iTween.MoveBy (label, iTween.Hash ("amount", new Vector3(0f, 60f*scale, 0f), "time", 0.3f));
	}

}
