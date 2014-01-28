using System;
using System.IO;
using UnityEngine;
using UnityEditor;

public static class AutoBuild
{
	[MenuItem("AutoBuild/Build Server")]
	public static void Server()
	{
		string[] server = new string[] { "Assets/Scenes/Server_default.unity" };

		BuildTarget target;
		string extension = "";
		
		switch (Application.platform)
		{
		case RuntimePlatform.WindowsEditor: target = BuildTarget.StandaloneWindows; extension = ".exe"; break;
		case RuntimePlatform.OSXEditor: target = BuildTarget.StandaloneOSXIntel; extension = ".app"; break;
		default: return;
		}
		
		BuildPipeline.BuildPlayer(server, "Build/mac/lg_server" + extension, target, BuildOptions.AutoRunPlayer);
	}

	[MenuItem("AutoBuild/Build Client")]
	public static void Client()
	{
		string[] client = new string[] { "Assets/Scenes/Play.unity" };

		BuildTarget target;
		string extension = "";
		
		switch (Application.platform)
		{
		case RuntimePlatform.WindowsEditor: target = BuildTarget.StandaloneWindows; extension = ".exe"; break;
		case RuntimePlatform.OSXEditor: target = BuildTarget.StandaloneOSXIntel; extension = ".app"; break;
		default: return;
		}
		
		BuildPipeline.BuildPlayer(client, "Build/mac/LonelyGalaxy" + extension, target, BuildOptions.AutoRunPlayer);
	}

	[MenuItem("AutoBuild/Build Both")]
	public static void Both()
	{
		string[] server = new string[] { "Assets/Scenes/Server_default.unity" };
		string[] client = new string[] { "Assets/Scenes/Play.unity" };
		
		BuildTarget target;
		string extension = "";
		
		switch (Application.platform)
		{
		case RuntimePlatform.WindowsEditor: target = BuildTarget.StandaloneWindows; extension = ".exe"; break;
		case RuntimePlatform.OSXEditor: target = BuildTarget.StandaloneOSXIntel; extension = ".app"; break;
		default: return;
		}
		
		BuildPipeline.BuildPlayer(server, "Build/mac/lg_server" + extension, target, BuildOptions.AutoRunPlayer);
		BuildPipeline.BuildPlayer(client, "Build/mac/LonelyGalaxy" + extension, target, BuildOptions.AutoRunPlayer);
	}
}
