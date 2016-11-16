using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class InventoryUI : MonoBehaviour {

	public bool isOpen;
	public GameObject inventoryMenu;
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

	public void ChangeTab(){
		tabs.Find(x => x.Key.isOn).Value.SetActive(true);
		foreach(var item in tabs.FindAll(x => !x.Key.isOn)){
			item.Value.SetActive(false);
		}
	}

	public void Toggle() {
		isOpen = !isOpen;
		inventoryMenu.SetActive(!inventoryMenu.activeSelf);
		if(isOpen){
			FindObjectOfType<Cast>().canCast = false;
		}else if(FindObjectOfType<Cast>().canCast == false && FindObjectOfType<Cast>().hasCasted == false){
			FindObjectOfType<Cast>().canCast = true;
		}
	}
}
