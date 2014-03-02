using UnityEngine;
using System.Collections;

public class APIObject {

	public string raw;

	public int id;
	public string name;
	public bool networked;

	public Vector3 position;
	public Vector3 rotation;
	public Quaternion quaternion;
	public Vector3 scale;

	public StatCollection stats;
	public ResourceCollection resources;


	public APIObject (string _raw) {
		raw = _raw;
		fromHashtable(MiniJSON.Json.Hashtable(_raw));
	}

	public void fromHashtable (Hashtable hash) {
//		Hashtable attributes = new Hashtable((IDictionary)hash);
		int.TryParse(hash["id"].ToString(), out id);
		name = hash["name"].ToString();
		ParsePosition (new Hashtable((IDictionary)hash["position"]));
		ParseRotation (new Hashtable((IDictionary)hash["rotation"]));
		ParseScale (new Hashtable((IDictionary)hash["scale"]));
		ParseStats (new Hashtable((IDictionary)hash["stats"]));
		ParseResources (new Hashtable((IDictionary)hash["resources"]));
	}

	void ParsePosition (Hashtable _position) {
		position = Vector3.zero;
		float.TryParse(_position["x"].ToString(), out position.x);
		float.TryParse(_position["y"].ToString(), out position.y);
		float.TryParse(_position["z"].ToString(), out position.z);
	}

	void ParseRotation (Hashtable _rotation) {
		rotation = Vector3.zero;
		float.TryParse(_rotation["x"].ToString(), out rotation.x);
		float.TryParse(_rotation["y"].ToString(), out rotation.y);
		float.TryParse(_rotation["z"].ToString(), out rotation.z);
		quaternion = Quaternion.identity;
		quaternion.eulerAngles = rotation;
	}

	void ParseScale (Hashtable _scale) {
		scale = Vector3.zero;
		float.TryParse(_scale["x"].ToString(), out scale.x);
		float.TryParse(_scale["y"].ToString(), out scale.y);
		float.TryParse(_scale["z"].ToString(), out scale.z);
	}

	void ParseStats (Hashtable _stats) {
		stats = new StatCollection(_stats);
	}

	void ParseResources (Hashtable _resources) {
		resources = new ResourceCollection(_resources);
	}

	public override string ToString () {
		return "[APIObject: " + raw + "]";
	}

}
