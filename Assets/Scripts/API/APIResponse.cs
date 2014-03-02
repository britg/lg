using UnityEngine;
using System.Collections;

public class APIResponse {

	public string raw;
	public object receiver;

	Hashtable hashtable;

	public APIResponse (string _raw) {
		raw = _raw;
	}

	public string Get (string key) {
		toHashtable();
		return (string) hashtable[key];
	}

	public APIObject GetObject () {
		return new APIObject(raw);
	}

	public Hashtable toHashtable () {
		if (hashtable == null) {
			hashtable = MiniJSON.Json.Hashtable(raw);
		}
		return hashtable;
	}

	public override string ToString () {
		return raw;
	}

	public object this[string key] {
		get {
			toHashtable();
			return hashtable[key];
		}
	}

}
