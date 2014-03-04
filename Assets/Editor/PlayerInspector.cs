using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PlayerEditor))]
public class PlayerInspector : Editor {

	GameObject go;
	PlayerEditor playerEditor;

	public override void OnInspectorGUI () {
		playerEditor = (PlayerEditor)target;

		DrawDefaultInspector();

//		EditorGUILayout.BeginHorizontal();
//		EditorGUILayout.PrefixLabel("Speed");
//		playerEditor.speed = EditorGUILayout.Slider(playerEditor.speed, 0f, 200f); 
//		EditorGUILayout.EndHorizontal();

		Rect r = EditorGUILayout.BeginHorizontal("Button");
		if(GUI.Button(r, "Save")) {
			PersistEdits();
		}
		EditorGUILayout.PrefixLabel("Save");
		EditorGUILayout.EndHorizontal();

	}

	void PersistEdits () {
		go = GameObject.Find ("GalaxyBuilder");
		if (go == null) {
			go = new GameObject("GalaxyBuilder");
			go.AddComponent<APIBehaviour>();
		}
		APIBehaviour api = go.GetComponent<APIBehaviour>();
		api.urlBase = LG.dbHost + "/api";
		api.Post("/players/template", playerEditor.toFormData(), PersistEditsReturn, PersistEditsReturn);
	}

	void PersistEditsReturn (APIResponse response) {
		DestroyImmediate(go);
	}

}
