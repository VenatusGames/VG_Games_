using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MapController : MonoBehaviour {

	public GameObject menuObj;
	bool isOpen;
	public Map currentMap;
	public List<Map> maps;
	public GameObject currentMapObject;
	[HideInInspector]
	public int idToBuy,idToTravel;
	public GameObject travelPopup;

	public enum MapType{
		lake,
		tropical,
		all
	}
	void Start(){
		int mapID = PlayerPrefs.GetInt("Level", 0);
		Map loadedMap = maps[mapID];
		currentMap = loadedMap;
		Destroy(currentMapObject);
		currentMapObject = (GameObject)Instantiate(loadedMap.mapObj,loadedMap.mapObj.transform.position,Quaternion.identity);
		RenderSettings.skybox = currentMap.skybox;
		Save();
	}

	void Save(){
		PlayerPrefs.SetInt("Level",currentMap.ID);
		PlayerPrefs.Save();
		Invoke("Save",5f);
	}

	public void Toggle(){
		isOpen = !isOpen;
		menuObj.SetActive(isOpen);
		if(isOpen){
			FindObjectOfType<Cast>().canCast = false;
		}else if(FindObjectOfType<Cast>().canCast == false && FindObjectOfType<Cast>().hasCasted == false){
			FindObjectOfType<Cast>().canCast = true;
		}
	}

	public void ChangeMap(bool a = false){
		Destroy(currentMapObject);
		Map loadedMap = maps[idToTravel];
		GameObject mapObj = loadedMap.mapObj;
		travelPopup.SetActive(false);
		Toggle();
		FindObjectOfType<MainMenu>().ToggleMainMenu();
		currentMap = loadedMap;
		currentMapObject = (GameObject)Instantiate(mapObj,mapObj.transform.position,Quaternion.identity);
		RenderSettings.skybox = currentMap.skybox;
		FindObjectOfType<BaitSelector>().PopulateSlider();
	}
}
