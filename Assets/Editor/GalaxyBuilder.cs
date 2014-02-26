﻿using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using UnityEngine;
using UnityEditor;

public static class GalaxyBuilder {

	public static GameObject go;

	[MenuItem("Lonely Galaxy/Persist Selection %&b")]
	public static void PersistGalaxy() {

//		Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.TopLevel);
//		UnityEngine.Debug.Log(Selection.activeObject);
//		string fullName = Selection.activeObject.ToString();
//		string apiName = fullName.Split(new string[] {" - "}, System.StringSplitOptions.None)[0];
//		Vector3 pos = Selection.activeTransform.position;
//		Selection.objects = selection;
		
		
		go = new GameObject("GalaxyBuilder");
		PersistenceRequest api = (PersistenceRequest)go.AddComponent<PersistenceRequest>();
		WWWForm postData = new WWWForm();

		int i = 0;
		GameObject galaxy = GameObject.Find ("Galaxy");
		foreach (Transform child in galaxy.transform) {
			postData.AddField("o[][name]", child.gameObject.name);
			postData.AddField("o[][x]", child.transform.position.x.ToString());
			postData.AddField("o[][y]", child.transform.position.y.ToString());
			postData.AddField("o[][z]", child.transform.position.z.ToString());
			postData.AddField("o[][scale_x]", child.transform.localScale.x.ToString());
			postData.AddField("o[][scale_y]", child.transform.localScale.y.ToString());
			postData.AddField("o[][scale_z]", child.transform.localScale.z.ToString());
			postData.AddField("o[][rotation_x]", child.transform.eulerAngles.x.ToString());
			postData.AddField("o[][rotation_y]", child.transform.eulerAngles.y.ToString());
			postData.AddField("o[][rotation_z]", child.transform.eulerAngles.z.ToString());
			i++;
		}
		api.urlBase = LG.dbHost + "/api";
		api.Post ("/galaxy", postData, OnPersistSuccess, OnPersistError);
	}

	static void OnPersistSuccess (Hashtable response, GameObject receiver) {
		UnityEngine.Debug.Log("Success");
		Reset();
	}

	static void OnPersistError (string response, GameObject receiver) {
		UnityEngine.Debug.Log("Error: " + response);
		Reset();
	}

	static void Reset () {
		GameObject.DestroyImmediate(go);
	}

}
