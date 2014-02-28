using UnityEngine;
using System.Collections;

[System.Serializable]
public class StatCollection {

	public Hashtable statsTable;

	public void Set (Hashtable _stats) {
		statsTable = new Hashtable();
		foreach (DictionaryEntry pair in _stats) {
			statsTable[pair.Key] = new Stat(pair);
		}
	}

	public Stat Get (string name) {
		return (Stat)statsTable[name];
	}

	public WWWForm toFormData () {
		WWWForm formData = new WWWForm();
		foreach (DictionaryEntry pair in statsTable) {
			string k = "player[properties][" + pair.Key.ToString() + "]";
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
}
