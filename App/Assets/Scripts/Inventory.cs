using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class Inventory : MonoBehaviour {

	public List<BaitType> baits;
	public List<BobberType> bobbers;
	public List<RodType> rods;

	public List<MapType> ownedMaps;

	public int money;
	public Text moneyText;

	void Start(){
		Text savedText = moneyText;
		InventorySave loadedSave = new InventorySave();
		if(System.IO.File.Exists(Application.persistentDataPath + @"\inventory.json")){
			string jsonSaveString = System.IO.File.ReadAllText(Application.persistentDataPath + @"\inventory.json");
			JsonUtility.FromJsonOverwrite(jsonSaveString,loadedSave);
			loadedSave.LoadInventory(this,loadedSave);
		}else{
			System.IO.File.Create(Application.persistentDataPath + @"\inventory.json");
		}
		if(!ownedMaps.Exists(x => x.ID == FindObjectOfType<ItemDatabase>().maps[0].ID)){
			ownedMaps.Add(FindObjectOfType<ItemDatabase>().maps[0]);
		}
		moneyText = savedText;
		SaveInventory();
	}

	void Update(){
		if(moneyText != null)
			moneyText.text = money + "g";
	}

	public void SaveInventory(){
		InventorySave save = new InventorySave(baits,bobbers,rods,ownedMaps);
		string jsonSaveString = JsonUtility.ToJson(save,true);
		if(System.IO.File.Exists(Application.persistentDataPath + @"\inventory.json")){
			System.IO.File.WriteAllText(Application.persistentDataPath + @"\inventory.json",jsonSaveString);
		}else{
			System.IO.File.Create(Application.persistentDataPath + @"\inventory.json");
			System.IO.File.WriteAllText(Application.persistentDataPath + @"\inventory.json",jsonSaveString);
		}
		Invoke("SaveInventory",5);
	}

	public void AddBait(BaitType bait, int amount){
		if(baits.Exists(x => x.name == bait.name)){
			baits.Find(x => x.name == bait.name).amount += amount;
		}else{
			baits.Add(bait);
			baits.Last().amount = amount;
		}
	}

	public void AddBobber(BobberType lure){
		bobbers.Add(lure);
	}

	public void AddRod(RodType rod){
		rods.Add(rod);
	}

}
