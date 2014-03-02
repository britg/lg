using UnityEngine;
using System.Collections;

public class Resource {

	public string name;
	public int value;

	public Resource (string _name, int _value) {
		name = _name;
		value = _value;
	}

	public Resource (DictionaryEntry pair) {
		name = pair.Key.ToString();
		int.TryParse(pair.Value.ToString(), out value);
	}

	public Resource (string _name, string _vStr) {
		name = _name;
		int.TryParse(_vStr, out value);
	}

	public override string ToString () {
		return value.ToString();
	}

}
