using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class FishDatabase : MonoBehaviour {
	public List<Fish> fish;
	public List<Fish> weightedFishArray;

	public void FakeCatch(){
		print(GetRandomFish().fishName);
	}

	public Fish GetRandomFish(){
		List<int> fishIds = new List<int>();
		foreach (var item in fish) {
			if(item.map == FindObjectOfType<MapController>().currentMap.mapType || item.map == MapController.MapType.all){
				fishIds.Add(fish.FindIndex(x => x.fishName == item.fishName));
			}
		}

		List<Fish> fishList = new List<Fish>();
		foreach (var item in fishIds) {
			fishList.Add(fish[item]);
		}
		int totalRarity = 0;
		foreach (var item in fishList) {
			totalRarity += item.rarity;
		}
		while (true) {
			int rand = Random.Range(0,totalRarity+1);
			int cumulative = 0;
			for (int i = 0; i < fishList.Count; i++) {

				cumulative += fishList[i].rarity;
				if(rand < cumulative){
					return fishList[i];
				}
			}
		}
		//Old method
//		weightedFishArray = new List<Fish>();
//		List<int> fishIds = new List<int>();
//		foreach (var item in fish) {
//			if(item.map == FindObjectOfType<MapController>().currentMap.mapType || item.map == MapController.MapType.all){
//				fishIds.Add(fish.FindIndex(x => x.fishName == item.fishName));
//			}
//		}
//		List<Fish> fishList = new List<Fish>();
//		foreach (var item in fishIds) {
//			fishList.Add(fish[item]);
//		}
//		int maxRarity = -1000;
//
//		foreach (var item in fishList) {
//			if(item.rarity == 0)
//				item.rarity = 1;
//			if(item.rarity > maxRarity) {
//				maxRarity = item.rarity;
//			}
//
//		}
//
//		foreach (Fish item in fishList) {
//			int timesToAdd = Mathf.Abs(item.rarity - (maxRarity+1));
//
//			for (int i = 0; i < timesToAdd; i++) {
//				weightedFishArray.Add(item);
//			}
//		}
	}

	public void TestRarity(){		
		List<Fish> caught = new List<Fish>();
		for (int i = 0; i < 100; i++) {
			
			caught.Add(GetRandomFish());
		}
		foreach (var item in caught.Distinct().ToList()) {
			print(item.fishName+" Has been caught: "+ caught.Count(x=>x.fishName == item.fishName) +" Times");
		}
	}

	public void Load(){
		FishDatabaseSave load = new FishDatabaseSave(new List<Fish>());
		LevelIdentifier level = FindObjectOfType<LevelIdentifier>();
		if(System.IO.File.Exists(Application.persistentDataPath + @"\"+level.levelName+"fishdatabase.json")){
			string jsonSaveString = System.IO.File.ReadAllText(Application.persistentDataPath + @"\"+level.levelName+"fishdatabase.json");
			if(jsonSaveString.Length > 10){
				JsonUtility.FromJsonOverwrite(jsonSaveString,load);
			}
		}else{
			System.IO.File.Create(Application.persistentDataPath + @"\"+level.levelName+"fishdatabase.json");
		}
		fish = load.fish;
	}

	public void Save(){
		FishDatabaseSave load = new FishDatabaseSave(fish);
		LevelIdentifier level = FindObjectOfType<LevelIdentifier>();
		string jsonSaveString = JsonUtility.ToJson(load,true);
		System.IO.File.WriteAllText(Application.persistentDataPath + @"\"+level.levelName+"fishdatabase.json",jsonSaveString);
	}
}
