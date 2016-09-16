using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FishDatabase))]
public class FishDatabaseEditor : Editor {
	public override void OnInspectorGUI(){
		DrawDefaultInspector();

		FishDatabase database = (FishDatabase)target;
		if(GUILayout.Button("Save")){
			database.Save();
		}
		if(GUILayout.Button("Load")){
			database.Load();
		}
	}
}
