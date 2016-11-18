using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class ExperienceSave{
	public float currentXP;
	public int level;
	public ExperienceSave(float _a, int _b){
		currentXP = _a;
		level = _b;
	}
}
[System.Serializable]
public class FishDatabaseSave{
	public List<Fish> fish;

	public FishDatabaseSave(List<Fish> a){
		fish = a;
	}
}
[System.Serializable]
public class MissionSave{
	public List<Mission> missions;

	public MissionSave(List<Mission> a){
		missions = a;
	}
}

[System.Serializable]
public class InventorySave {
	public List<int> baitIDs;
	public List<int> baitAmounts;
	public List<int> bobbers;
	public List<int> rods;
	public List<int> maps;
	public int equippedBobber,equippedRod;

	public InventorySave(){
		baitIDs = new List<int>();
		bobbers = new List<int>();
		rods = new List<int>();
		maps = new List<int>();
	}

	public InventorySave(List<BaitType> a,List<BobberType> b,List<RodType> c, List<MapType> d){
		baitIDs = new List<int>();
		baitAmounts = new List<int>();
		bobbers = new List<int>();
		rods = new List<int>();
		maps = new List<int>();
		try {
			
			equippedBobber = b.Find(x => x.isEquipped).ID;
			equippedRod = c.Find(x => x.isEquipped).ID;

		} catch (System.Exception ex) {
			equippedBobber = 0;
			equippedRod = 0;
			Debug.Log(ex);
		}

		foreach (var item in a) {
			int id = item.ID;
			int amount = item.amount;
			baitIDs.Add(id);
			baitAmounts.Add(amount);
		}
		foreach (var item in b) {
			bobbers.Add(item.ID);
		}
		foreach (var item in c) {
			rods.Add(item.ID);
		}
		foreach (var item in d) {
			maps.Add(item.ID);
		}
	}

	public void LoadInventory(Inventory inv, InventorySave save){
		inv.baits = LoadBaits(save);
		inv.rods = LoadRods(save);
		inv.bobbers = LoadBobbers(save);
		inv.ownedMaps = LoadMaps(save);
	}
    List<BaitType> LoadBaits(InventorySave save) {
        List<BaitType> loadedBaits = new List<BaitType>();
        if (save.baitIDs.Count > 0)
        {
            for (int i = 0; i < save.baitIDs.Count; i++)
            {
                loadedBaits.Add(Object.FindObjectOfType<ItemDatabase>().baits[save.baitIDs[i]]);
            }
            for (int i = 0; i < save.baitAmounts.Count; i++)
            {
                loadedBaits[i].amount = save.baitAmounts[i];
            }
        }
		return loadedBaits;
	}
	List<RodType> LoadRods(InventorySave save){
		List<RodType> loadedRods = new List<RodType>();
        if (save.rods.Count > 0)
        {
            foreach (var item in save.rods)
            {
                loadedRods.Add(Object.FindObjectOfType<ItemDatabase>().rods[item]);
            }
            if (loadedRods.Count > 0)
                loadedRods.Find(x => x.ID == save.equippedRod).isEquipped = true;
        }
		return loadedRods;
	}
	List<BobberType> LoadBobbers(InventorySave save){
		List<BobberType> loadedBobbers = new List<BobberType>();
        if (save.bobbers.Count > 0)
        {
            foreach (var item in save.bobbers)
            {
                loadedBobbers.Add(Object.FindObjectOfType<ItemDatabase>().bobbers[item]);
            }
            try
            {
                loadedBobbers.Find(x => x.ID == save.equippedBobber).isEquipped = true;
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex);
            }
        }
		return loadedBobbers;
	}
	List<MapType> LoadMaps(InventorySave save){
		List<MapType> loadedMaps = new List<MapType>();
        if (save.maps.Count > 0)
        {
            foreach (var item in save.maps)
            {
                loadedMaps.Add(Object.FindObjectOfType<ItemDatabase>().maps[item]);
            }
        }
		return loadedMaps;
	}
}
