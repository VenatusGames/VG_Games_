using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
public class Shop : MonoBehaviour {
	public bool isOpen;
	public GameObject shopObject;
	public List<GameObject> tabMenus;
	public List<Toggle> toggles;
	public List<KeyValuePair<Toggle,GameObject>> tabs = new List<KeyValuePair<UnityEngine.UI.Toggle, GameObject>>();
	private Color defaultColor;
	public Color toggleColor;
	public RectTransform contentRect;


	void Start(){
		contentRect.position = Vector3.zero;
		for (int i = 0; i < toggles.Count; i++) {
			tabs.Add(new KeyValuePair<Toggle, GameObject>(toggles[i],tabMenus[i]));
		}
		defaultColor = toggles[0].colors.normalColor;
	}

	void Update(){
		foreach (var item in toggles) {
			if(item.isOn){
				ColorBlock oldBlock = item.colors;
				oldBlock.normalColor = toggleColor;
				item.colors = oldBlock;
			}else{
				ColorBlock oldBlock = item.colors;
				oldBlock.normalColor = defaultColor;
				item.colors = oldBlock;
			}
		}
		ChangeTab();
		if(!contentRect.gameObject.activeSelf){
			contentRect.position = Vector3.zero;
		}
		if(isOpen && !FindObjectOfType<Cast>().hasCasted)
			FindObjectOfType<Cast>().canCast = false;
	}

	public void Toggle(){
		isOpen = !isOpen;
		shopObject.SetActive(!shopObject.activeSelf);
		if(isOpen){
			FindObjectOfType<Cast>().canCast = false;
		}else if(FindObjectOfType<Cast>().canCast == false && FindObjectOfType<Cast>().hasCasted == false){
			FindObjectOfType<Cast>().canCast = true;
		}
	}

	public void ChangeTab(){
		tabs.Find(x => x.Key.isOn).Value.SetActive(true);
		foreach(var item in tabs.FindAll(x => !x.Key.isOn)){
			item.Value.SetActive(false);
		}
	}

	public void BuyBait(BaitType bait, int amount){
		print(bait.ToString() + ":" + amount);
		FindObjectOfType<Inventory>().money -= amount * bait.price;
		FindObjectOfType<Inventory>().AddBait(bait,amount);
	}

	public void BuyLure(LureType lure, int amount){
		print(lure.ToString() + ":" + amount);
		FindObjectOfType<Inventory>().money -= amount * lure.price;

	}

	public void BuyRod(RodType rod){
		print(rod.ToString());
		FindObjectOfType<Inventory>().money -= rod.price;

	}

}
