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
		foreach (Fish item in fish) {
			for (int i = 0; i < item.rarity; i++) {
				weightedArray.Add(item);
			}
		}
		return weightedArray[Random.Range(0,weightedArray.Count)];
	}
}
