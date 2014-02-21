using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using UnityEngine;
using UnityEditor;

public static class GalaxyBuilder {

	public static GameObject go;

	[MenuItem("Lonely Galaxy/Persist Selection %&p")]
	public static void PersistSelection() {

//		Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.TopLevel);
		UnityEngine.Debug.Log(Selection.activeObject);
		string fullName = Selection.activeObject.ToString();
		string apiName = fullName.Split(new string[] {" - "}, System.StringSplitOptions.None)[0];
		Vector3 pos = Selection.activeTransform.position;
//		Selection.objects = selection;
		
		
		go = new GameObject("GalaxyBuilder");
		PersistenceRequest api = (PersistenceRequest)go.AddComponent<PersistenceRequest>();
		WWWForm postData = new WWWForm();
		postData.AddField("spawn[name]", apiName);
		postData.AddField("spawn[x]", pos.x.ToString());
		postData.AddField("spawn[y]", pos.y.ToString());
		postData.AddField("customizations[source]", "editor");
		api.urlBase = LG.dbHost + "/api";
		api.Post ("/spawns", postData, OnPersistSuccess, OnPersistError);
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
		GameObject.DestroyImmediate(Selection.activeObject);
		GameObject.DestroyImmediate(go);
	}

}
