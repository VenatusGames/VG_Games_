using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

[ExecuteInEditMode]
public class ShopListing : MonoBehaviour {
	public enum ItemChoice
	{
		bait,
		bobber,
		rod
	}
	public bool canBuy;
	public ItemChoice type;
	public int itemID;

	public Text nameText,priceText,descText;

	void Start() {
		switch (type) {

		case ItemChoice.bait:
			if(itemID >= 0 && itemID < FindObjectOfType<ItemDatabase>().baits.Count){
				BaitType curB = FindObjectOfType<ItemDatabase>().baits[itemID];
				nameText.text = curB.name;
				priceText.text = curB.price.ToString() + "g";
			}else{
				nameText.text = "";
				priceText.text = "";
			}
			break;

		case ItemChoice.bobber:
			if(itemID >= 0 && itemID < FindObjectOfType<ItemDatabase>().bobbers.Count){
				BobberType curL = FindObjectOfType<ItemDatabase>().bobbers[itemID];
				nameText.text = curL.name;
				priceText.text = curL.price.ToString() + "g";
				descText.text = "";
			}else{
				nameText.text = "";
				priceText.text = "";
				descText.text = "";
			}
			break;

		case ItemChoice.rod:
			if(itemID >= 0 && itemID < FindObjectOfType<ItemDatabase>().rods.Count){
				RodType curR = FindObjectOfType<ItemDatabase>().rods[itemID];
				nameText.text = curR.name;
				priceText.text = curR.price.ToString() + "g";
				descText.text = "";
			}else{
				nameText.text = "";
				priceText.text = "";
				descText.text = "";
			}
			break;

		default:
			break;
		}
	}

	void Update(){
		if(type == ItemChoice.rod){
			Inventory inv = FindObjectOfType<Inventory>();
			if(inv.rods.Exists(x => x.ID == itemID)){
				if(inv.rods.Find(x => x.ID == itemID).isEquipped){
					priceText.text = "Equipped";
				}else{
					priceText.text = "Owned";
				}
			}
		}
		if(type == ItemChoice.bobber){
			Inventory inv = FindObjectOfType<Inventory>();
			if(inv.bobbers.Exists(x => x.ID == itemID)){
				if(inv.bobbers.Find(x => x.ID == itemID).isEquipped){
					priceText.text = "Equipped";
				}else{
					priceText.text = "Owned";
				}
			}
		}
	}

	public void Buy(int amount){
		int money = FindObjectOfType<Inventory>().money;
		switch (type) {

		case ItemChoice.bait: 
			BaitType curB = FindObjectOfType<ItemDatabase>().baits[itemID];
			if((curB.price * amount) < money){
				FindObjectOfType<Shop>().BuyBait(curB,amount);
			}else{
				int maxAmount = Mathf.FloorToInt(money/curB.price);
				maxAmount = Mathf.Clamp(maxAmount,0,amount);
				FindObjectOfType<Shop>().BuyBait(curB,maxAmount);
			}
			break;

		case ItemChoice.bobber: 
			BobberType curL = FindObjectOfType<ItemDatabase>().bobbers[itemID];
			if(FindObjectOfType<Inventory>().bobbers.Exists(x => x.ID == curL.ID)){
				FindObjectOfType<RodSwitcher>().SwitchBobber(curL);
			}else if(money > curL.price){
				FindObjectOfType<Shop>().BuyLure(curL);
			}
			break;

		case ItemChoice.rod: 
			RodType curR = FindObjectOfType<ItemDatabase>().rods[itemID];
			if(FindObjectOfType<Inventory>().rods.Exists(x => x.ID == curR.ID)){
				FindObjectOfType<RodSwitcher>().SwitchRod(curR);
			}else if(money > curR.price){
				FindObjectOfType<Shop>().BuyRod(curR);
			}
			break;

		default:
			break;
		}
	}

	void OnValidate(){
		Start();
	}
}
