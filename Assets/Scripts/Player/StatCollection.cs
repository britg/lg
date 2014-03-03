using UnityEngine;
using System.Collections;

[System.Serializable]
public class StatCollection {

	public Hashtable statsTable = new Hashtable();

	public StatCollection (Hashtable _stats) {
		Set (_stats);
	}

	public void Set (Hashtable _stats) {
		statsTable = new Hashtable();
		foreach (DictionaryEntry pair in _stats) {
			statsTable[pair.Key] = new Stat(pair);
		}
	}

	public void Set (string statName, float value) {
		statsTable[statName] = new Stat(statName, value);
	}

	public void Set (string[] rpcSerialization) {
		int i = 0;
		for (i = 0; i < rpcSerialization.Length/2; i++) {
			string key = rpcSerialization[i];
			string vStr = rpcSerialization[i+1];
			statsTable[key] = new Stat(key, vStr);
		}
	}

	public Stat Get (string name) {
		return (Stat)statsTable[name];
	}

	public Stat this[string statName] {
		get {
			return Get (statName);
		}
	}

	public WWWForm toFormData () {
		WWWForm formData = new WWWForm();
		foreach (DictionaryEntry pair in statsTable) {
			string k = "o[s][" + pair.Key.ToString() + "]";
			string v = Get(pair.Key.ToString()).ToString();
			formData.AddField(k, v);
		}

		return formData;
	}

	public string[] toRPCSerialization () {
		string[] arr = new string[statsTable.Count*2];
		int i = 0;
		foreach (DictionaryEntry pair in statsTable) {
			arr[i] = pair.Key.ToString();
			i++;
			arr[i] = pair.Value.ToString();
			i++;
		}

		return arr;
	}

	public override string ToString () {
		string s = "";
		foreach (DictionaryEntry pair in statsTable) {
			s += "{" + pair.Key.ToString() + ": " + pair.Value.ToString() + "}";
		}
		return s;
	}

	public IDictionaryEnumerator GetEnumerator () {
		return statsTable.GetEnumerator();
	}

}
