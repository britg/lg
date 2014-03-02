using UnityEngine;
using System.Collections;

public class APIResponse {

	public string raw;
	public object receiver;

	Hashtable hashtableCache;

	public APIResponse (string _raw) {
		raw = _raw;
	}

	public Hashtable toHashtable () {
		hashtableCache = MiniJSON.Json.Hashtable(raw);
		return hashtableCache;
	}

	public override string ToString () {
		return raw;
	}

	public object this[string key] {
		get {
			if (hashtableCache == null) {
				toHashtable();
			}
			return hashtableCache[key];
		}
	}

}
