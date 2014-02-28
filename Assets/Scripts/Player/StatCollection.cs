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
			string k = "player[" + pair.Key.ToString() + "]";
			string v = Get(pair.Key.ToString()).ToString();
			formData.AddField(k, v);
		}

		return formData;
	}
}
