using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MissionDatabase))]
public class MissionEditor : Editor {
	public override void OnInspectorGUI(){
		DrawDefaultInspector();

		MissionDatabase database = (MissionDatabase)target;
		if(GUILayout.Button("Save")){
			database.Save();
		}
		if(GUILayout.Button("Load")){
			database.Load();
		}
	}
}
