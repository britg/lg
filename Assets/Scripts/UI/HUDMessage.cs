using UnityEngine;
using System.Collections;

public class HUDMessage  {

	public string text;
	public Color color = Color.white;
	public float duration = 1f;

	public HUDMessage (string _text) {
		text = _text;
	}

}
