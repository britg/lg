using UnityEngine;
using System.Collections;

public static class LG  {

	public static Player player;
	public static PlayerProcessor playerProcessor;
	public static Plane plane = new Plane(Vector3.up, Vector3.right, Vector3.down);
	public static Hashtable cache = new Hashtable();

	// Speed percentage when out of fuel
	public static float thrustFactor = 0.1f;

	public static string version = "0.0.21";
	public static string patchHost = "http://lg.foolishaggro.com/system/_output";

	public static string dbHost = "lg.foolishaggro.dev";

	/* Player Notifications */
	public const string n_playerShouldSpawn = "OnPlayerShouldSpawn";
	public const string n_playerSpawned = "OnPlayerSpawn";
	public const string n_playerKey = "player";
	public const string n_playerLoaded = "OnPlayerLoaded";
	public const string n_playerStatsLoaded = "OnPlayerStatsLoaded";
	public const string n_playerHit = "OnPlayerHit";
	public const string n_registerWeapon = "OnRegisterWeapon";

	/* End Player Notifications */

	/* Ship Notifications */
	public const string n_shipThrustStart = "OnShipThrustStart";
	public const string n_shipThrustEnd = "OnShipThrustEnd";
	
	/* GUI Notifications */

	public const string n_showServerConnectionUI = "OnShowServerConnectionUI";
	public const string n_hideServerConnectionUI = "OnHideServerConnectionUI";
	public const string n_AddLabel = "OnAddLabel";

	/* End GUI Notifications */

	/* Server Startup Notifications */
	public const string n_cameraAnchored = "OnCameraAnchored";
	public const string n_worldObjectsSpawned = "OnWorldObjectsSpawned";


	public const string m_outOfFuel = "Out of Fuel - Limited to Thrusters";

	public static Hashtable Hash (params object[] args) {
		Hashtable hashTable = new Hashtable(args.Length/2);
		if (args.Length %2 != 0) {
			Debug.LogError("Hash requires an even number of arguments!"); 
			return null;
		} else {
			int i = 0;
			while(i < args.Length - 1) {
				hashTable.Add(args[i], args[i+1]);
				i += 2;
			}
			return hashTable;
		}
	}

}
