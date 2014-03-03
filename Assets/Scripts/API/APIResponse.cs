using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class APIResponse {

	public string raw;
	public object receiver;

	Hashtable _hashtable;
	Hashtable hashtable {
		get {
			if (_hashtable == null) {
				_hashtable = MiniJSON.Json.Hashtable(raw);
			}
			return _hashtable;
		}
	}

	public APIResponse (string _raw) {
		raw = _raw;
	}

	public string Get (string key) {
		return (string) hashtable[key];
	}

	public APIObject GetObject () {
		return new APIObject(raw);
	}

	public List<APIObject> GetObjects () {
		List<object> objects = (List<object>)hashtable["objects"];
		List<APIObject> apiObjects = new List<APIObject>();
		foreach (object o in objects) {
			apiObjects.Add(new APIObject((IDictionary)o));
		}

		return apiObjects;
	}

	public override string ToString () {
		return raw;
	}

	public object this[string key] {
		get {
			return hashtable[key];
		}
	}

}
