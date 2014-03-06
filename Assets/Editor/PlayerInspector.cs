﻿using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PlayerEditor))]
public class PlayerInspector : Editor {

	GameObject go;
	APIBehaviour api;
	PlayerEditor playerEditor;

	public override void OnInspectorGUI () {
		playerEditor = (PlayerEditor)target;

		DrawDefaultInspector();

		Rect r = EditorGUILayout.BeginHorizontal("Button");
		if(GUI.Button(r, "Save")) {
			PersistEdits();
		}
		EditorGUILayout.PrefixLabel("");
		EditorGUILayout.EndHorizontal();

	}

	void PersistEdits () {
		go = GameObject.Find ("GalaxyBuilder");
		if (go == null) {
			go = new GameObject("GalaxyBuilder");
			go.AddComponent<APIBehaviour>();
		}
		api = go.GetComponent<APIBehaviour>();
		api.urlBase = LG.dbHost + "/api";
		api.Post("/players/template", playerEditor.toFormData(), PersistEditsReturn, PersistEditsReturn);
		EditorUtility.DisplayCancelableProgressBar("Saving...", "Saving...", 0);
		EditorUtility.ClearProgressBar();
	}

	void PersistEditsReturn (APIResponse response) {
		DestroyImmediate(go);
	}

}
