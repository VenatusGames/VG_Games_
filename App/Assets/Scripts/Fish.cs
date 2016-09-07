using UnityEngine;
using System.Collections;

[System.Serializable]
public class Fish {
	
	public GameObject fishPrefab;
	public string fishName;
	public float minBobTime,maxBobTime,minCatchTime,maxCatchTime;
	public Color rarityColour;
	public float minWeight,maxWeight,minLength,maxLength;
	public float baseWeight,baseLength;
	public int basePrice;
}
