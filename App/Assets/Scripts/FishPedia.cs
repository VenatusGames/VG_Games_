using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class FishPedia : MonoBehaviour {

	public bool isOpened = false;
	public GameObject fishPedia;
	public FishList fishList = new FishList();
	public RectTransform fishPediaRect;

	public GameObject fishPediaListingPrefab;
	public List<FishPediaListing> listings = new List<FishPediaListing>();
	void Start(){
		fishPediaRect.localPosition = new Vector3(0,0,0);
		if(System.IO.File.Exists(Application.persistentDataPath + @"\save.json")){
			string jsonSaveString = System.IO.File.ReadAllText(Application.persistentDataPath + @"\save.json");
			JsonUtility.FromJsonOverwrite(jsonSaveString,fishList);
		}else{
			System.IO.File.Create(Application.persistentDataPath + @"\save.json");
		}
		isOpened = false;
		SaveFishpedia();

		Vector2 newSize = fishPediaRect.sizeDelta;
		newSize.y = (30)+(FindObjectOfType<FishDatabase>().fish.Count * 30);
		newSize.y = Mathf.Clamp(newSize.y,183,Mathf.Infinity);
		fishPediaRect.sizeDelta = newSize;

		FishDatabase database = FindObjectOfType<FishDatabase>();
		for (int i = 0; i < database.fish.Count; i++) {
			GameObject cur = (GameObject)Instantiate(fishPediaListingPrefab,fishPediaRect.transform);
			cur.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0,-30+(-30*i),0);
			cur.GetComponent<RectTransform>().localEulerAngles = Vector3.zero;
			cur.GetComponent<RectTransform>().localScale = Vector3.one;
			cur.GetComponent<FishPediaListing>().fish = new FishpediaFish(database.fish[i].fishName,0,0);
			listings.Add(cur.GetComponent<FishPediaListing>());
		}
		List<FishpediaFish> bestFish = UniqueFishArray(fishList.fishList);
		foreach (var item in bestFish) {
			if(listings.Find(x => x.fish.fishName == item.fishName)){
				FishPediaListing listing = listings.Find(x => x.fish.fishName == item.fishName);
				listing.isCaught = true;
				listing.fish.weight = item.weight;
				listing.fish.length = item.length;
			}
		}
	}

	void Update(){
		if(isOpened && !FindObjectOfType<Cast>().hasCasted)
			FindObjectOfType<Cast>().canCast = false;
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
				FishPediaListing listing = listings.Find(x => x.fish.fishName == fish.fishName);
				if(!listing.isCaught){
					listing.isCaught = true;
				}

				listing.UpdateListing(fish.weight,fish.length);
			}

			foreach (var item in listings) {
				item.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,item.GetComponent<RectTransform>().anchoredPosition.y,0);
				if(!item.isCaught){
					item.UpdateListing(0,0);
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
			FishpediaFish highestFish = new FishpediaFish("",0,0);
			foreach (var fish in tempFishList) {
				if((fish.weight + fish.length) > highest){
					highestFish = fish;
					highest = fish.weight + fish.length;
				}
			}
			returnList.Add(highestFish);
		}

		return returnList;
	}

	public void SaveFishpedia(){
		string jsonSaveString = JsonUtility.ToJson(fishList,true);
		System.IO.File.WriteAllText(Application.persistentDataPath + @"\save.json",jsonSaveString);
		Invoke("SaveFishpedia",5);
	}
}
