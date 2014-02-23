using UnityEngine;
using System.Collections;

public class WorldObject : LGMonoBehaviour {

	public int worldObjectId;
	protected string rawAttributes;
	public Hashtable attributes;
	public IDictionary properties;

	void Awake () {
		transform.parent = GameObject.Find ("WorldObjects").transform;
	}

	public void AssignAttributes (string _rawAttributes) {
		rawAttributes = _rawAttributes;
		attributes = MiniJSON.Json.Hashtable(rawAttributes);
		properties = (IDictionary)attributes["properties"];
		ExtractInt ("id", out worldObjectId);
	}

	public void ExtractInt (string name, out int local) {
		int.TryParse(attributes[name].ToString (), out local);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
