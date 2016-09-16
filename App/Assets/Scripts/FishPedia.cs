using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
public class FishPedia : MonoBehaviour {

	public bool isOpened = false;
	public GameObject fishPedia;
	public FishList fishList = new FishList();
	public RectTransform fishPediaRect;

	public GameObject fishPediaListingPrefab;
	public List<FishPediaListing> listings = new List<FishPediaListing>();
	public GameObject uiCanvas;
	int oldMask;
	MainMenu menu;
	void Start(){
		oldMask = Camera.main.cullingMask;
		menu = FindObjectOfType<MainMenu>();
		fishPediaRect.localPosition = new Vector3(0,0,0);
		if(System.IO.File.Exists(Application.persistentDataPath + @"\fishpedia.json")){
			string jsonSaveString = System.IO.File.ReadAllText(Application.persistentDataPath + @"\fishpedia.json");
			JsonUtility.FromJsonOverwrite(jsonSaveString,fishList);
		}else{
			System.IO.File.Create(Application.persistentDataPath + @"\fishpedia.json");
		}
		isOpened = false;
		SaveFishpedia();

		Vector2 newSize = fishPediaRect.sizeDelta;
		newSize.y = (30)+(FindObjectOfType<FishDatabase>().fish.Count * 30);
		newSize.y = Mathf.Clamp(newSize.y,183,Mathf.Infinity);
		fishPediaRect.sizeDelta = newSize;

		FishDatabase database = FindObjectOfType<FishDatabase>();
		List<Fish> oldDatabaseFish = database.fish;
		List<Fish> databaseFish = new List<Fish>();
		foreach (var fish in oldDatabaseFish) {
			if(fish.basePrice != 0){
				databaseFish.Add(fish);
			}
		}
		int y = 0;
		int z = 0;
		for (int i = 0; i < databaseFish.Count; i++) {
			GameObject cur = (GameObject)Instantiate(fishPediaListingPrefab,fishPediaRect.transform);
			cur.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(10+(z*110),-35+(y*-110),0);
			cur.GetComponent<RectTransform>().localEulerAngles = Vector3.zero;
			cur.GetComponent<RectTransform>().localScale = Vector3.one;
			cur.GetComponent<FishPediaListing>().fish = new FishpediaFish(databaseFish[i].fishName,0);
			cur.transform.FindChild("Border").GetComponent<Image>().color = databaseFish[i].rarityColour;
			GameObject child = cur.transform.FindChild("FishPosition").gameObject;
			GameObject fish = (GameObject)Instantiate(databaseFish[i].fishPrefab,child.GetComponent<RectTransform>().position,cur.transform.rotation,child.transform);
			fish.GetComponentInChildren<MeshRenderer>().gameObject.layer = LayerMask.NameToLayer("UI");
			fish.transform.localScale = new Vector3(fish.transform.localScale.x * 50f,fish.transform.localScale.y * 50f,fish.transform.localScale.z * 50f);
			fish.transform.localPosition = new Vector3(0,0,-40);
			listings.Add(cur.GetComponent<FishPediaListing>());
			z++;
			int mod = i + 1;
			if(mod % 5 == 0){
				y++;
				z = 0;
			}
		}
		List<FishpediaFish> bestFish = UniqueFishArray(fishList.fishList);
		foreach (var item in bestFish) {
			if(listings.Find(x => x.fish.fishName == item.fishName)){
				FishPediaListing listing = listings.Find(x => x.fish.fishName == item.fishName);
				listing.isCaught = true;
				listing.fish.weight = item.weight;
			}
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
			List<FishpediaFish> bestFish = UniqueFishArray(fishList.fishList);

			foreach (var fish in bestFish) {
				FishPediaListing listing = null;
				foreach (var item in listings) {
					if(item.fish.fishName == fish.fishName){
						listing = item;
					}
				}
				if(listing != null){
					if(!listing.isCaught){
						listing.isCaught = true;
					}

					listing.UpdateListing(fish.weight);
				}else{
					print(fish.fishName);
				}
			}

			foreach (var item in listings) {
				if(!item.isCaught){
					item.UpdateListing(0);
				}
			}

			//FishDatabase database = FindObjectOfType<FishDatabase>();
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

	public void SaveFishpedia(){
		string jsonSaveString = JsonUtility.ToJson(fishList,true);
		System.IO.File.WriteAllText(Application.persistentDataPath + @"\fishpedia.json",jsonSaveString);
		Invoke("SaveFishpedia",5);
	}
}
