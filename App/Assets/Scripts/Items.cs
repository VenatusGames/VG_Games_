using System.Collections;
using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class BaitType {
	public int ID;
	public string name;
	public int price,amount;
	public Sprite image;
	public float hookChanceBuff;
	public int maxMapID;
	public int rarityBuff;
	public BaitType(){
		ID = -1;
		name = "Default";
		price = 0;
		amount = 0;
		maxMapID = 0;
		rarityBuff = 0;
		hookChanceBuff = 0;
	}
}
[System.Serializable]
public class BaitFinder {
	public int ID;
	public int amount;
}
[System.Serializable]
public class BobberFinder {
	public int ID;
}
[System.Serializable]
public class RodFinder {
	public int ID;
}

[System.Serializable]
public class BobberType {
	public int ID;
	public string name;
	public int price;
	public bool isEquipped;
	public GameObject bobber;
}
[System.Serializable]
public class RodType {
	public int ID;
	public string name;
	public int price;
	public bool isEquipped;
	public Texture rodTexture;
	public GameObject rod;
}

[System.Serializable]
public class MapType {
	public int ID;
	public string name;
}

[System.Serializable]
public class Map {
	public int ID;
	public GameObject mapObj;
	public MapController.MapType mapType;
	public Material skybox;
}


