using UnityEngine;
using System.Collections;

[System.Serializable]
public class Fish {
	
	public GameObject fishPrefab;
	public string fishName;
	public float minBobTime,maxBobTime,minCatchTime,maxCatchTime;
	public Color rarityColour;
	public float minWeight,maxWeight;
	public float baseWeight;
	public int basePrice;
	public int baseXP;
	public int rarity;
}
