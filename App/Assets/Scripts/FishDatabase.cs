using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FishDatabase : MonoBehaviour {
	public List<Fish> fish;

	public Fish GetRandomFish(){
		List<Fish> weightedArray = new List<Fish>();
		int bestRarity = 0;
		foreach (Fish item in fish) {
			if(item.rarity > bestRarity){
				bestRarity = item.rarity;
			}
		}
		foreach (Fish item in fish) {
			int rarity = bestRarity = item.rarity;
			if(rarity == 0)
				rarity ++;
			
			for (int i = 0; i < rarity; i++) {
				weightedArray.Add(item);
			}
		}
		return weightedArray[Random.Range(0,weightedArray.Count)];
	}

	public void Load(){
		FishDatabaseSave load = new FishDatabaseSave(new List<Fish>());
		if(System.IO.File.Exists(Application.persistentDataPath + @"\fishdatabase.json")){
			string jsonSaveString = System.IO.File.ReadAllText(Application.persistentDataPath + @"\fishdatabase.json");
			if(jsonSaveString.Length > 10){
				JsonUtility.FromJsonOverwrite(jsonSaveString,load);
			}
		}else{
			System.IO.File.Create(Application.persistentDataPath + @"\fishdatabase.json");
		}
		fish = load.fish;
	}

	public void Save(){
		FishDatabaseSave load = new FishDatabaseSave(fish);
		string jsonSaveString = JsonUtility.ToJson(load,true);
		System.IO.File.WriteAllText(Application.persistentDataPath + @"\fishdatabase.json",jsonSaveString);
	}
}
