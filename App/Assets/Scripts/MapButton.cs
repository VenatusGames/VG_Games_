using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapButton : MonoBehaviour {
	public Image icon;
	public GameObject buyPopup,travelPopup;
	public Sprite x,buy,locked;
	public int id,cost;
	public int minLevel;

	private ExperienceSystem xpSys;
	private Inventory inv;
	public enum MapButtonState
	{
		open,
		canBuy,
		locked
	}
	public MapButtonState state;

	void Start(){
		inv = FindObjectOfType<Inventory>();
		xpSys = FindObjectOfType<ExperienceSystem>();
	}

	void Update(){

		if(minLevel <= xpSys.level){
			if(inv.ownedMaps.Exists(x => x.ID == FindObjectOfType<ItemDatabase>().maps[id].ID)){
				state = MapButtonState.open;
			}else{
				state = MapButtonState.canBuy;
			}
		}else{
			state = MapButtonState.locked;
		}


		if(state == MapButtonState.canBuy){
			icon.sprite = buy;
		}else if(state == MapButtonState.locked){
			icon.sprite = locked;
		}else{
			icon.sprite = x;
		}
	}

	public void Travel(){
		if(state == MapButtonState.canBuy){
			BuyPopup();
		}else if(state == MapButtonState.locked){
			
		}else{
			TravelPopup();
		}
	}

	public void Buy(){
		if(FindObjectOfType<Inventory>().money > cost){
			FindObjectOfType<Inventory>().money -= cost;
			FindObjectOfType<Inventory>().ownedMaps.Add(FindObjectOfType<ItemDatabase>().maps[FindObjectOfType<MapController>().idToBuy]);
			BuyPopup();
		}
	}

	public void BuyPopup(){
		buyPopup.SetActive(!buyPopup.activeSelf);
		if(buyPopup.activeSelf){
			FindObjectOfType<MapController>().idToBuy = id;
		}
	}

	public void TravelPopup(){
		travelPopup.SetActive(!travelPopup.activeSelf);
		if(travelPopup.activeSelf){
			FindObjectOfType<MapController>().idToTravel = id;
		}
	}

	public void ChangeMap(){
		FindObjectOfType<MapController>().ChangeMap();
	}
}
