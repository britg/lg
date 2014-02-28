// (c)2011 MuchDifferent. All Rights Reserved.

using System;
using System.Collections;
using UnityEngine;
using uLink;

public class ServerManager : uLink.MonoBehaviour
{

	public int port = 7100;
	public int maxConnections = 64;
	public bool cleanupAfterPlayers = true;
	public bool registerHost = false;
	public int targetFrameRate = 60;
	public bool dontDestroyOnLoad = false;

	public PlayerSpawner playerSpawner;

	void Start()
	{
		Application.targetFrameRate = targetFrameRate;

		if (dontDestroyOnLoad) DontDestroyOnLoad(this);
		
		uLink.Network.InitializeServer(maxConnections, port);
	}
	
	void uLink_OnServerInitialized()
	{
		Debug.Log("Server successfully started on port " + uLink.Network.listenPort);
		
		if (registerHost) uLink.MasterServer.RegisterHost();
	}
	
	void uLink_OnPlayerDisconnected(uLink.NetworkPlayer player)
	{
		if (cleanupAfterPlayers)
		{
			uLink.Network.DestroyPlayerObjects(player);
			uLink.Network.RemoveRPCs(player);
			
			// this is not really necessery unless you are removing NetworkViews without calling uLink.Network.Destroy
			uLink.Network.RemoveInstantiates(player);
		}
	}
	
	void uLink_OnPlayerConnected(uLink.NetworkPlayer player)
	{
		playerSpawner.StartPlayer(player);
	}
}
