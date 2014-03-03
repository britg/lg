using UnityEngine;
using System.Collections;

[System.Serializable]
public class ResourceCollection {
	
	public Hashtable resourcesTable = new Hashtable();

	public ResourceCollection () {

	}

	public ResourceCollection (Hashtable r) {
		Set (r);
	}
	
	public void Set (Hashtable _resources) {
		resourcesTable = new Hashtable();
		foreach (DictionaryEntry pair in _resources) {
			resourcesTable[pair.Key] = new Resource(pair);
		}
	}
	
	public void Set (string statName, int value) {
		resourcesTable[statName] = new Resource(statName, value);
	}
	
	public void Set (string[] rpcSerialization) {
		int i = 0;
		for (i = 0; i < rpcSerialization.Length/2; i++) {
			string key = rpcSerialization[i];
			string vStr = rpcSerialization[i+1];
			resourcesTable[key] = new Resource(key, vStr);
		}
	}
	
	public Stat Get (string name) {
		return (Stat)resourcesTable[name];
	}
	
	public Stat this[string statName] {
		get {
			return Get (statName);
		}
	}
	
	public WWWForm toFormData () {
		WWWForm formData = new WWWForm();
		foreach (DictionaryEntry pair in resourcesTable) {
			string k = "o[r][" + pair.Key.ToString() + "]";
			string v = Get(pair.Key.ToString()).ToString();
			formData.AddField(k, v);
		}
		
		return formData;
	}
	
	public string[] toRPCSerialization () {
		string[] arr = new string[resourcesTable.Count*2];
		int i = 0;
		foreach (DictionaryEntry pair in resourcesTable) {
			arr[i] = pair.Key.ToString();
			i++;
			arr[i] = pair.Value.ToString();
			i++;
		}
		
		return arr;
	}
	
	public override string ToString () {
		string s = "";
		foreach (DictionaryEntry pair in resourcesTable) {
			s += "{" + pair.Key.ToString() + ": " + pair.Value.ToString() + "}";
		}
		return s;
	}

	public IDictionaryEnumerator GetEnumerator () {
		return resourcesTable.GetEnumerator();
	}

	
}
