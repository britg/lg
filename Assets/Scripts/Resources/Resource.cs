using UnityEngine;
using System.Collections;

public class Resource {

	/* Temp resource names */
	public static string tier1 = "tier1";
	public static string tier2 = "tier2";
	public static string tier3 = "tier3";
	public static string tier4 = "tier4";
	public static string tier5 = "tier5";
	public static string tier6 = "tier6";
	public static string tier7 = "tier7";

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
