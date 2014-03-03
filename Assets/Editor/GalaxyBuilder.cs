using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using UnityEngine;
using UnityEditor;

public static class GalaxyBuilder {

	public static GameObject go;

	[MenuItem("Lonely Galaxy/Persist Galaxy %&b")]
	public static void PersistGalaxy() {

//		Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.TopLevel);

//		UnityEngine.Debug.Log(Selection.activeObject);
//		string fullName = Selection.activeObject.ToString();
//		string apiName = fullName.Split(new string[] {" - "}, System.StringSplitOptions.None)[0];
//		Vector3 pos = Selection.activeTransform.position;
//		Selection.objects = selection;

		if (EditorApplication.currentScene != "Assets/Scenes/GalaxyBuilder.unity") {
			return;
		}

		go = GameObject.Find ("GalaxyBuilder");
		if (go == null) {
			go = new GameObject("GalaxyBuilder");
			go.AddComponent<APIBehaviour>();
		}
		APIBehaviour api = go.GetComponent<APIBehaviour>();
		WWWForm postData = new WWWForm();

		int i = 0;
		GameObject galaxy = GameObject.Find ("Galaxy");
		foreach (Transform child in galaxy.transform) {

			WorldObject wo = child.gameObject.GetComponent<WorldObject>();
			StatCollection stats = wo.stats;
			ResourceCollection resources = wo.resources;
			uLink.NetworkView networkView = child.GetComponent<uLink.NetworkView>();
			string networked = (networkView != null ? "1" : "0");

			postData.AddField("o[][name]", TemplateName(child.gameObject.name));
			postData.AddField("o[][networked]", networked);
			postData.AddField("o[][x]", child.transform.position.x.ToString());
			postData.AddField("o[][y]", child.transform.position.y.ToString());
			postData.AddField("o[][z]", child.transform.position.z.ToString());
			postData.AddField("o[][scale][x]", child.transform.localScale.x.ToString());
			postData.AddField("o[][scale][y]", child.transform.localScale.y.ToString());
			postData.AddField("o[][scale][z]", child.transform.localScale.z.ToString());
			postData.AddField("o[][rotation][x]", child.transform.eulerAngles.x.ToString());
			postData.AddField("o[][rotation][y]", child.transform.eulerAngles.y.ToString());
			postData.AddField("o[][rotation][z]", child.transform.eulerAngles.z.ToString());

			foreach (DictionaryEntry pair in stats) {
				string k = "o[][stats][" + pair.Key.ToString() + "]";
				string v = pair.Value.ToString();
				postData.AddField(k, v);
			}
			
			foreach (DictionaryEntry pair in resources) {
				string k = "o[][resources][" + pair.Key.ToString() + "]";
				string v = pair.Value.ToString();
				postData.AddField(k, v);
			}

			i++;
		}
		api.urlBase = LG.dbHost + "/api";
		api.Post ("/galaxy", postData, OnPersistSuccess, OnPersistError);
	}

	static string TemplateName (string goName) {
		return goName.Split(new string[]{" - "}, StringSplitOptions.RemoveEmptyEntries)[0];
	}

	static void OnPersistSuccess (APIResponse response) {
		UnityEngine.Debug.Log("Success");
		Reset();
	}

	static void OnPersistError (APIResponse response) {
		UnityEngine.Debug.Log("Error: " + response);
		Reset();
	}

	static void Reset () {
		GameObject.DestroyImmediate(go);
	}

}
