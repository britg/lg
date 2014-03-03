using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StatCollection {

	public Hashtable statsTable = new Hashtable();

	List<string> statsToSync = new List<string>();

	// Common stats
	public float Fuel {
		get {
			return Get (LG.s_fuel).value;
		}
	}

	public float FuelBurn {
		get {
			return Get (LG.s_fuelBurn).value;
		}
	}

	public StatCollection () {

	}

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
		if (!statsToSync.Contains(statName)) {
			statsToSync.Add (statName);
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

	public float Add (string name, float amount) {
		float v = Get (name).value + amount;
		Set (name, v);
		return v;
	}

	public float Remove (string name, float amount) {
		return Add (name, -amount);
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

	public bool ShouldSync () {
		return statsToSync.Count > 0;
	}

	public string[] StatsToSync () {
		string[] arr = new string[statsToSync.Count*2];
		int i = 0;
		foreach (string statName in statsToSync) {
			arr[i] = statName;
			i++;
			arr[i] = Get(statName).value.ToString();
			i++;
		}

		return arr;
	}

	public void FlushStatsToSync () {
		statsToSync = new List<string>();
	}

}
