using UnityEngine;
using System.Collections;

[System.Serializable]
public class Stat {
	public string name;
	public float value;

	public Stat (string _name, float _value) {
		name = _name;
		value = _value;
	}

	public Stat (DictionaryEntry keyPair) {
		name = (string)keyPair.Key;
		float.TryParse(keyPair.Value.ToString(), out value);
	}

	public override string ToString () {
		return value.ToString();
	}
}
