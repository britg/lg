using System;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;

public static class AutoBuild
{
	[MenuItem("AutoBuild/Patch")]
	public static void Both () {
		ClientMac();
		ClientWin();
	}

	[MenuItem("AutoBuild/Client Mac")]
	public static void ClientMac () {
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneOSXIntel);
		string[] client = new string[] { "Assets/Scenes/Launch.unity", "Assets/Scenes/Play.unity" };
		BuildPipeline.BuildPlayer(client, "Patchie/_current/lg_client_mac.app", BuildTarget.StandaloneOSXIntel, BuildOptions.AutoRunPlayer);
	}

	[MenuItem("AutoBuild/Client Win")]
	public static void ClientWin () {
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneWindows);
		string[] client = new string[] { "Assets/Scenes/Launch.unity", "Assets/Scenes/Play.unity" };
		BuildPipeline.BuildPlayer(client, "Patchie/_current/lg_client_win.exe", BuildTarget.StandaloneWindows, BuildOptions.AutoRunPlayer);
	}

	[MenuItem("AutoBuild/Server Mac %&s")]
	public static void ServerMac () {
		string[] server = new string[] { "Assets/Scenes/Server_default.unity" };
		BuildPipeline.BuildPlayer(server, "Build/mac/lg_server.app", BuildTarget.StandaloneOSXIntel, BuildOptions.AutoRunPlayer);
	}


	[MenuItem("AutoBuild/Server Linux")]
	public static void ServerLinux () {
		string[] server = new string[] { "Assets/Scenes/Server_default.unity" };
		BuildTarget target = BuildTarget.StandaloneLinux64;
		BuildPipeline.BuildPlayer(server, "../lg_server/lg_server.x86_64", target, BuildOptions.None);
	}
}
