using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ResourceCollection {
	
	public Hashtable resourcesTable = new Hashtable();
	List<string> resourcesToSync = new List<string>();

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
	
	public void Set (string resourceName, int value) {
		resourcesTable[resourceName] = new Resource(resourceName, value);
		if (!resourcesToSync.Contains(resourceName)) {
			resourcesToSync.Add (resourceName);
		}
	}
	
	public Resource Get (string name) {
		return (Resource)resourcesTable[name];
	}
	
	public Resource this[string resourceName] {
		get {
			return Get (resourceName);
		}
	}

	public int Add (string name, int amount) {
		int v = Get (name).value + amount;
		Set (name, v);
		return v;
	}

	public int Add (Resource res) {
		return Add (res.name, res.value);
	}
	
	public int Remove (string name, int amount) {
		return Add (name, -amount);
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
	
	public bool ShouldSync () {
		return resourcesToSync.Count > 0;
	}
	
	public string[] ResourcesToSync () {
		string[] arr = new string[resourcesToSync.Count*2];
		int i = 0;
		foreach (string resourceName in resourcesToSync) {
			arr[i] = resourceName;
			i++;
			arr[i] = Get(resourceName).value.ToString();
			i++;
		}
		
		return arr;
	}
	
	public void FlushResourcesToSync () {
		resourcesToSync = new List<string>();
	}
	
}
