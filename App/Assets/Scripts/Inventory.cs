using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class Inventory : MonoBehaviour {

	public List<BaitType> baits;
	public List<LureType> lures;
	public List<RodType> rods;

	public int money;
	public Text moneyText;
	void Update(){
		moneyText.text = money + "g";
	}

	public void AddBait(BaitType bait, int amount){
		if(baits.Exists(x => x.name == bait.name)){
			baits.Find(x => x.name == bait.name).amount += amount;
		}else{
			baits.Add(bait);
			baits.Last().amount = amount;
		}
	}
	public void AddLure(LureType lure, int amount){
		if(lures.Exists(x => x.name == lure.name)){
			lures.Find(x => x.name == lure.name).amount += amount;
		}else{
			lures.Add(lure);
			lures.Last().amount = amount;
		}
	}
}
