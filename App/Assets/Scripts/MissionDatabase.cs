using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MissionDatabase : MonoBehaviour {
	public List<Mission> missions;


	public void Save(){
		MissionSave load = new MissionSave(missions);
		string jsonSaveString = JsonUtility.ToJson(load,true);
		System.IO.File.WriteAllText(Application.persistentDataPath + @"\missiondatabase.json",jsonSaveString);
	}

	public void Load(){
		MissionSave load = new MissionSave(new List<Mission>());
		if(System.IO.File.Exists(Application.persistentDataPath + @"\missiondatabase.json")){
			string jsonSaveString = System.IO.File.ReadAllText(Application.persistentDataPath + @"\missiondatabase.json");
			if(jsonSaveString.Length > 10){
				JsonUtility.FromJsonOverwrite(jsonSaveString,load);
			}
		}else{
			System.IO.File.Create(Application.persistentDataPath + @"\missiondatabase.json");
		}
		missions = load.missions;
	}
}
