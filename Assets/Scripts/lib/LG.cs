using UnityEngine;
using System.Collections;

public static class LG  {

	public static string version = "0.0.21";
	public static string patchHost = "http://lg.foolishaggro.com/system/_output";

	public static string dbHost = "lg.foolishaggro.dev";

	/* Player Messages */
	public const string n_playerShouldSpawn = "OnPlayerShouldSpawn";
	public const string n_playerSpawned = "OnPlayerSpawn";
	public const string n_playerKey = "player";
	public const string n_playerLoaded = "OnPlayerLoaded";
	public const string n_playerStatsLoaded = "OnPlayerStatsLoaded";
	public const string n_playerHit = "OnPlayerHit";

	/* End Player Messages */

	/* Ship Messages */
	public const string n_shipThrustStart = "OnShipThrustStart";
	public const string n_shipThrustEnd = "OnShipThrustEnd";
	
	/* GUI Messages */

	public const string n_showServerConnectionUI = "OnShowServerConnectionUI";
	public const string n_hideServerConnectionUI = "OnHideServerConnectionUI";

	/* End GUI Messages */

	/* Server Startup Messages */
	public const string n_worldObjectsSpawned = "OnWorldObjectsSpawned";

	/* Stat names */
	public const string s_shields = "shields";
	public const string s_hull = "hull";
	public const string s_speed = "speed";
	public const string s_fuel = "fuel";
	public const string s_fuelBurn = "fuel_burn";
	public const string s_extractorStrength = "extractor_strength";
	public const string s_extractorLength = "extractor_length";


	/* Resource names */
	public const string r1 = "white";
	public const string r2 = "green";
	public const string r3 = "blue";
	public const string r4 = "yellow";
	public const string r5 = "orange";
	public const string r6 = "red";
	public const string r7 = "black";


}
