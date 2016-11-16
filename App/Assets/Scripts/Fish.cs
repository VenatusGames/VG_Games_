using UnityEngine;
using System.Collections;

[System.Serializable]
public class Fish {
	
	public GameObject fishPrefab;
	public string fishName;
	[Range(0,1)]
	public float catchChance;
	public Color rarityColour;
	public float minWeight,maxWeight;
	public float baseWeight;
	public int basePrice;
	public int baseXP;
	[Range(0,100)]
	public int rarity;
	public int rarityLevel;
	public string bio;
	public bool isJunk;
	public MapController.MapType map;
	public int strength;
	public int width;
}
