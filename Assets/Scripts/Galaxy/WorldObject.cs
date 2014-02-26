using UnityEngine;
using System.Collections;

public class WorldObject : LGMonoBehaviour {

	public int worldObjectId;
	public Hashtable attributes;
	public IDictionary properties;

	static Hashtable prefabCache = new Hashtable();
	public static GameObject galaxy;

	public static void PlaceObjects (IList objects) {
		foreach (var obj in objects) {
			
			Hashtable attributes = new Hashtable((IDictionary)obj);
			bool networked = (bool)attributes["networked"];
			
			if (networked) {
				if (uLink.Network.isServer) {
					PlaceNetworkedObject(attributes);
				}
			} else {
				PlaceStaticObject(attributes);
			}
		}	
	}

	static void PlaceStaticObject (Hashtable attributes) {
		string name 			= (string)attributes["name"];
		Vector3 pos 		  	= WorldObject.ExtractPosition(attributes);
		
		GameObject cached = (GameObject)prefabCache[name];
		if (cached == null) {
			prefabCache[name] = (GameObject)Resources.Load (name);
			cached = (GameObject)prefabCache[name];
		}
		
		GameObject serverObj = (GameObject)Instantiate(cached, pos, Quaternion.identity);
		serverObj.transform.parent = galaxy.transform;
		serverObj.SendMessage("AssignAttributes", attributes, SendMessageOptions.DontRequireReceiver);
	}
	
	static void PlaceNetworkedObject (Hashtable attributes) {
		string name 			= (string)attributes["name"];
		string serverName 		= name + " - Server";
		string proxyName 		= name + " - Proxy";
		Vector3 pos 		  	= WorldObject.ExtractPosition(attributes);
		
		GameObject serverPrefab = (GameObject)prefabCache[serverName];
		GameObject proxyPrefab = (GameObject)prefabCache[proxyName];
		if (serverPrefab == null) {
			prefabCache[serverName] = (GameObject)Resources.Load (serverName);
			serverPrefab = (GameObject)prefabCache[serverName];
		}
		if (proxyPrefab == null) {
			prefabCache[proxyName] = (GameObject)Resources.Load (proxyName);
			proxyPrefab = (GameObject)prefabCache[proxyName];
		}
		
		GameObject serverObj = uLink.Network.Instantiate(proxyPrefab, serverPrefab, pos, Quaternion.identity, 0);
		serverObj.transform.parent = galaxy.transform;
		serverObj.SendMessage("AssignAttributes", attributes, SendMessageOptions.DontRequireReceiver);
	}

	public static Vector3 ExtractPosition (Hashtable attributes) {
		string xStr = (string)attributes["x"];
		string yStr = (string)attributes["y"];
		string zStr = (string)attributes["z"];
		Vector3 pos = Vector3.zero;
		float.TryParse(xStr, out pos.x);
		float.TryParse(yStr, out pos.y);
		float.TryParse(zStr, out pos.z);
		
		return pos;
	}
	
	public static Vector3 ExtractScale (Hashtable attributes) {
		string xStr = (string)attributes["scale_x"];
		string yStr = (string)attributes["scale_y"];
		string zStr = (string)attributes["scale_z"];
		Vector3 scale = Vector3.zero;
		float.TryParse(xStr, out scale.x);
		float.TryParse(yStr, out scale.y);
		float.TryParse(zStr, out scale.z);
		
		return scale;
	}
	
	public static Vector3 ExtractRotation (Hashtable attributes) {
		string xStr = (string)attributes["rotation_x"];
		string yStr = (string)attributes["rotation_y"];
		string zStr = (string)attributes["rotation_z"];
		Vector3 rot = Vector3.zero;
		float.TryParse(xStr, out rot.x);
		float.TryParse(yStr, out rot.y);
		float.TryParse(zStr, out rot.z);
		
		return rot;
	}

	public static int ExtractInt (Hashtable attributes, string name) {
		int local;
		int.TryParse(attributes[name].ToString (), out local);
		return local;
	}

	public void AssignAttributes (Hashtable attributes) {
		worldObjectId = WorldObject.ExtractInt(attributes, "id");
		transform.position = WorldObject.ExtractPosition(attributes);
		transform.localScale = WorldObject.ExtractScale(attributes);
		transform.eulerAngles = WorldObject.ExtractRotation(attributes);
	}

}
