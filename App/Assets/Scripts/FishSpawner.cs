using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FishSpawner : MonoBehaviour {
	List<Fish> fish;

	void Start(){
		fish = FindObjectOfType<FishDatabase>().fish;
	}

	public Fish GetRandomFish(){
		List<Fish> weightedArray = new List<Fish>();
		List<int> fishIds = new List<int>();
		foreach (var item in fish) {
			if(item.map == FindObjectOfType<MapController>().currentMap.mapType){
				fishIds.Add(fish.FindIndex(x => x.fishName == item.fishName));
			}
		}
		List<Fish> fishList = new List<Fish>();
		foreach (var item in fishIds) {
			fishList.Add(fish[item]);
		}

		foreach (Fish item in fishList) {
			for (int i = 0; i < item.rarity; i++) {
				weightedArray.Add(item);
			}
		}
		Fish retFish = weightedArray[Random.Range(0,weightedArray.Count)];
		if(retFish.map != FindObjectOfType<MapController>().currentMap.mapType){
			print("it fucked up");
		}
		return retFish;
	}
}
