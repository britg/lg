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
	public const string n_AddLabel = "OnAddLabel";

	/* End GUI Messages */

	/* Server Startup Messages */
	public const string n_worldObjectsSpawned = "OnWorldObjectsSpawned";

}
