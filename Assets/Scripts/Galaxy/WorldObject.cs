using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldObject : LGMonoBehaviour {

	public static GameObject galaxy;
	static Hashtable prefabCache = new Hashtable();
	static List<int> idCache = new List<int>();

	public int worldObjectId;
	public string worldObjectName;
	public StatCollection stats = new StatCollection();
	public ResourceCollection resources = new ResourceCollection();

	public static void PlaceObjects (List<APIObject> objects) {
		foreach (APIObject o in objects) {
			if (o.networked) {
				if (uLink.Network.isServer) {
					PlaceNetworkedObject(o);
				}
			} else {
				PlaceStaticObject(o);
			}
		}
	}

	public static void PlaceNetworkedObject (APIObject o) {
		GameObject serverPrefab = (GameObject)prefabCache[o.serverName];
		GameObject proxyPrefab = (GameObject)prefabCache[o.proxyName];
		if (serverPrefab == null) {
			prefabCache[o.serverName] = (GameObject)Resources.Load (o.serverName);
			serverPrefab = (GameObject)prefabCache[o.serverName];
		}
		if (proxyPrefab == null) {
			prefabCache[o.proxyName] = (GameObject)Resources.Load (o.proxyName);
			proxyPrefab = (GameObject)prefabCache[o.proxyName];
		}
		
		GameObject serverObj = uLink.Network.Instantiate(proxyPrefab, serverPrefab, o.position, o.quaternion, 0);
		serverObj.transform.parent = galaxy.transform;
		serverObj.SendMessage("ParseAPIObject", o, SendMessageOptions.RequireReceiver);
	}

	public static void PlaceStaticObject (APIObject o) {
		if (idCache.Contains(o.id)) {
			return;
		}

		GameObject cached = (GameObject)prefabCache[o.name];
		if (cached == null) {
			prefabCache[o.name] = (GameObject)Resources.Load (o.name);
			cached = (GameObject)prefabCache[o.name];
		}
		
		GameObject serverObj = (GameObject)Instantiate(cached, o.position, o.quaternion);
		idCache.Add(o.id);
		serverObj.transform.parent = galaxy.transform;
		serverObj.SendMessage("ParseAPIObject", o, SendMessageOptions.RequireReceiver);
	}

	void ParseAPIObject (APIObject o) {
		worldObjectId = o.id;
		worldObjectName = o.name;
		stats = o.stats;
		resources = o.resources;
		transform.localScale = o.scale;
	}

	public static string Server_TakeDamage = "TakeDamage";
	public virtual void TakeDamage (float amount) {}
}
