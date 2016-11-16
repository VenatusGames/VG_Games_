using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
public class FishPedia : MonoBehaviour {

	public bool isOpened = false;
	public bool isBioOpen = false;
	public GameObject fishPedia;
	public FishList fishList = new FishList();

	public GameObject fishPediaListingPrefab;
	public List<FishPediaListing> listings;
	public GameObject uiCanvas;
	int oldMask;
	MainMenu menu;
	public GameObject spawnedFish;

	//Bio Variables
	public GameObject bioPage;
	public int bioFishID;
	public Text bioName,bioLocation,bioDesc,bioWeight,bioCatches;


	void Start(){
		oldMask = Camera.main.cullingMask;
		menu = FindObjectOfType<MainMenu>();
		if(System.IO.File.Exists(Application.persistentDataPath + @"\fishpedia.json")){
			string jsonSaveString = System.IO.File.ReadAllText(Application.persistentDataPath + @"\fishpedia.json");
			JsonUtility.FromJsonOverwrite(jsonSaveString,fishList);
		}else{
			System.IO.File.Create(Application.persistentDataPath + @"\fishpedia.json");
		}
		isOpened = false;
		SaveFishpedia();

		FishDatabase database = FindObjectOfType<FishDatabase>();
		List<Fish> oldDatabaseFish = database.fish;
		List<Fish> databaseFish = new List<Fish>();
		foreach (var fish in oldDatabaseFish) {
			if(fish.basePrice != 0){
				databaseFish.Add(fish);
			}
		}
		listings = FindObjectsOfType<FishPediaListing>().ToList();
		foreach (var item in listings) {
			item.UpdateListing();
		}
	}

	void Update(){

		if(isOpened && !FindObjectOfType<Cast>().hasCasted)
			FindObjectOfType<Cast>().canCast = false;
		if(isOpened){
			Camera.main.orthographic = true;
			Camera.main.cullingMask = (1 << LayerMask.NameToLayer("UI"));
			Camera.main.nearClipPlane = -1000;
			menu.menuObject.SetActive(false);
			menu.menuButton.SetActive(false);
			uiCanvas.SetActive(false);
		}else if(menu.isOpen){
			Camera.main.orthographic = false;
			Camera.main.nearClipPlane = 0.01f;
			menu.menuObject.SetActive(true);
			menu.menuButton.SetActive(true);
			uiCanvas.SetActive(true);
		}else{
			Camera.main.orthographic = false;
			Camera.main.cullingMask = oldMask;
		}
		if(spawnedFish){
			spawnedFish.transform.Rotate(new Vector3(0,1,0));
		}
	}

	public void ToggleBio(int id = 0) {
		if(!isBioOpen){
			if(FindObjectOfType<FishPedia>().fishList.fishList.Exists(x => x.fishName == FindObjectOfType<FishDatabase>().fish[id].fishName)) {
				Destroy(spawnedFish);
				isBioOpen = !isBioOpen;
				bioPage.SetActive(!bioPage.activeSelf);
				bioFishID = id;
				if(isBioOpen){
					foreach (var item in listings) {
						item.gameObject.SetActive(false);
					}
					bioPage.transform.FindChild("FishPosition").transform.FindChild("Border").GetComponent<Image>().color = FindObjectOfType<FishDatabase>().fish[bioFishID].rarityColour;
					GameObject child = bioPage.transform.FindChild("FishPosition").gameObject;
					spawnedFish = (GameObject)Instantiate(FindObjectOfType<FishDatabase>().fish[bioFishID].fishPrefab,child.GetComponent<RectTransform>().position,child.transform.rotation,child.transform);
					spawnedFish.GetComponentInChildren<MeshRenderer>().gameObject.layer = LayerMask.NameToLayer("UI");
					spawnedFish.transform.localScale = new Vector3(spawnedFish.transform.localScale.x * 50f,spawnedFish.transform.localScale.y * 50f,spawnedFish.transform.localScale.z * 50f);
					spawnedFish.transform.localPosition = new Vector3(0,0,-500);

					bioName.text = FindObjectOfType<FishDatabase>().fish[id].fishName;
					bioLocation.text = CapFirstLetter(FindObjectOfType<FishDatabase>().fish[id].map.ToString());
					bioDesc.text = FindObjectOfType<FishDatabase>().fish[id].bio;
					bioWeight.text = "Best Weight: "+ UniqueFishArray(fishList.fishList).Find(x => x.fishName == FindObjectOfType<FishDatabase>().fish[id].fishName).weight+"Kg";
					bioCatches.text = "Caught "+FindObjectOfType<FishPedia>().fishList.fishList.FindAll(x => x.fishName == FindObjectOfType<FishDatabase>().fish[id].fishName).Count+" times";

				}else{
					foreach (var item in listings) {
						item.gameObject.SetActive(true);
					}
				}
			}
		}else{
			isBioOpen = !isBioOpen;
			bioPage.SetActive(!bioPage.activeSelf);
			if(!isBioOpen){
				foreach (var item in listings) {
					item.gameObject.SetActive(true);
				}
			}
		}
	}

	public void Toggle() {
		isOpened = !isOpened;
		fishPedia.SetActive(!fishPedia.activeSelf);
		if(isOpened){
			FindObjectOfType<Cast>().canCast = false;
		}else if(FindObjectOfType<Cast>().canCast == false && FindObjectOfType<Cast>().hasCasted == false){
			FindObjectOfType<Cast>().canCast = true;
		}


		if(isOpened){
			listings = FindObjectsOfType<FishPediaListing>().ToList();
			foreach (var item in listings) {
				item.UpdateListing();
			}

		}
	}

	List<FishpediaFish> UniqueFishArray(List<FishpediaFish> fishList){
		List<string> tempList = new List<string>();
		List<FishpediaFish> returnList = new List<FishpediaFish>();
		foreach (var fish in fishList) {
			if(!tempList.Exists(x => x == fish.fishName)){
				tempList.Add(fish.fishName);
			}
		}

		foreach (var item in tempList) {
			List<FishpediaFish> tempFishList = fishList.FindAll(x => x.fishName == item);
			float highest = Mathf.NegativeInfinity;
			FishpediaFish highestFish = new FishpediaFish("",0);
			foreach (var fish in tempFishList) {
				if((fish.weight) > highest){
					highestFish = fish;
					highest = fish.weight;
				}
			}
			returnList.Add(highestFish);
		}

		return returnList;
	}

	public string CapFirstLetter(string s){
		return char.ToUpper(s[0]) + s.Substring(1);
	}

	public void SaveFishpedia(){
		string jsonSaveString = JsonUtility.ToJson(fishList,true);
		System.IO.File.WriteAllText(Application.persistentDataPath + @"\fishpedia.json",jsonSaveString);
		Invoke("SaveFishpedia",5);
	}
}
