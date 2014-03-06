using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MobEditor))]
[InitializeOnLoad]
public class MobInspector : Editor {

	GameObject apiObj;
	APIBehaviour api;
	MobEditor mobEditor;

	void OnEnable () {
//		EditorApplication.update += Update;
	}

	void Update () {
//		Debug.Log ("update is " + EditorApplication.update);
	}
	
	public override void OnInspectorGUI () {
		mobEditor = (MobEditor)target;
		mobEditor.worldObjectName = TemplateName(mobEditor.gameObject.name);
		DrawDefaultInspector();

		if (PrefabUtility.GetPrefabType(mobEditor.gameObject) == PrefabType.PrefabInstance) {
			Rect r = EditorGUILayout.BeginHorizontal("Button");
			if(GUI.Button(r, "Save")) {
				PersistEdits();
			}
			EditorGUILayout.PrefixLabel("");
			EditorGUILayout.EndHorizontal();

			r = EditorGUILayout.BeginHorizontal("Button");
			if(GUI.Button(r, "Destroy")) {
				RemoveObject();
			}
			EditorGUILayout.PrefixLabel("");
			EditorGUILayout.EndHorizontal();
		}

		serializedObject.ApplyModifiedProperties();
		
	}

	string TemplateName (string inGameName) {
		return inGameName.Split(new string[]{" - "}, System.StringSplitOptions.RemoveEmptyEntries)[0];
	}
	
	void PersistEdits () {
		if (PrefabUtility.GetPrefabType(mobEditor.gameObject) == PrefabType.Prefab) {
			return;
		}

		apiObj = GameObject.Find ("GalaxyBuilder");
		if (apiObj == null) {
			apiObj = new GameObject("GalaxyBuilder");
			apiObj.AddComponent<APIBehaviour>();
		}
		api = apiObj.GetComponent<APIBehaviour>();
		api.urlBase = LG.dbHost + "/api";
		api.Put("/spawns/" + mobEditor.worldObjectId, mobEditor.toFormData("spawn"), PersistEditsReturn);
		EditorUtility.SetDirty(mobEditor);
		Repaint();
		SceneView.RepaintAll();
		EditorUtility.DisplayCancelableProgressBar("Test", "...", 0);
		EditorUtility.ClearProgressBar();
	}
	
	void PersistEditsReturn (APIResponse response) {
		Debug.Log ("Persist edits return");
		APIObject o = response.GetObject();
		mobEditor.worldObjectId = o.id;
		Repaint();
	}

	void RemoveObject () {

	}

}
