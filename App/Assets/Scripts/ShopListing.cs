using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ShopListing : MonoBehaviour {
	public enum ItemChoice
	{
		bait,
		lure,
		rod
	}
	public bool canBuy;
	public ItemChoice type;
	public int itemID;

	public Text nameText,priceText;

	void Start() {
		switch (type) {

		case ItemChoice.bait:
			BaitType curB = FindObjectOfType<ItemDatabase>().baits[itemID];
			nameText.text = curB.name;
			priceText.text = curB.price.ToString() + "g";
			break;

		case ItemChoice.lure: 
			LureType curL = FindObjectOfType<ItemDatabase>().lures[itemID];
			nameText.text = curL.name;
			priceText.text = curL.price.ToString() + "g";
			break;

		case ItemChoice.rod: 
			RodType curR = FindObjectOfType<ItemDatabase>().rods[itemID];
			nameText.text = curR.name;
			priceText.text = curR.price.ToString() + "g";
			break;

		default:
			break;
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

		case ItemChoice.lure: 
			LureType curL = FindObjectOfType<ItemDatabase>().lures[itemID];
			if((curL.price * amount) < money){
				FindObjectOfType<Shop>().BuyLure(curL,amount);
			}else{
				int maxAmount = Mathf.FloorToInt(money/curL.price);
				maxAmount = Mathf.Clamp(maxAmount,0,amount);
				FindObjectOfType<Shop>().BuyLure(curL,maxAmount);
			}
			break;

		case ItemChoice.rod: 
			RodType curR = FindObjectOfType<ItemDatabase>().rods[itemID];
			FindObjectOfType<Shop>().BuyRod(curR);
			break;

		default:
			break;
		}
	}
}
