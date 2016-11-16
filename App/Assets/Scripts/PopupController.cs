using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PopupController : MonoBehaviour{

	public GameObject uiPopup;
	public GameObject fishPopupObject;
	public Text popupFishName,popupFishWeight,popupFishPrice;
	public Text xpText;
	public Image popupBorder;
	private GameObject currentFish;
	public Queue<Popup> popupQueue = new Queue<Popup>();

	public GameObject currentPopup;
	public bool popupOpen;
	public int queueLength;
	void Update(){
		queueLength = popupQueue.Count;
		if(uiPopup.activeInHierarchy)
			fishPopupObject.transform.Rotate(new Vector3(0,2,0) * Time.deltaTime * 20);
	}

	public IEnumerator CreateFishPopup(Fish fish, bool late = false){
		if(!popupOpen){
			
			currentPopup = uiPopup;
			uiPopup.transform.localScale = new Vector3(0,0,1);
			uiPopup.SetActive(true);
			fishPopupObject.transform.eulerAngles = new Vector3(0,0,0);
			Destroy(currentFish);
			currentFish = (GameObject)Instantiate(fish.fishPrefab,fishPopupObject.transform.position,fish.fishPrefab.transform.rotation,fishPopupObject.transform);
			popupFishName.text = fish.fishName;
			popupBorder.color = fish.rarityColour;
			float weight = Round(Random.Range(fish.minWeight,fish.maxWeight),2);
			float weightDiffernce = (weight-fish.baseWeight);
			int price = Mathf.RoundToInt(fish.basePrice + (weightDiffernce));
			popupFishWeight.text = "Weight: "+weight.ToString()+ "Kg";
			popupFishPrice.text = "Price: " + price.ToString() + "g";
			FindObjectOfType<Inventory>().money += price;
			FindObjectOfType<FishPedia>().fishList.fishList.Add(new FishpediaFish(fish.fishName,weight));
			float questReward = FindObjectOfType<MissionController>().CheckFishForMissions(fish,weight);
			FindObjectOfType<ExperienceSystem>().AddXP((int)(fish.baseXP + weightDiffernce + questReward));
			xpText.text = "+"+(int)(fish.baseXP + weightDiffernce + questReward) + "xp";
			StartCoroutine(FadeOut());
			popupOpen = true;
			if(!late)
				popupQueue.Enqueue(new Popup(fish));
			float lerpTimer = 0;
			for (int i = 0; i < 50; i++) {
				uiPopup.transform.localScale = new Vector3(lerpTimer,lerpTimer,1);
				lerpTimer += 0.02f;
				yield return new WaitForEndOfFrame();
			}
		}else{
			popupQueue.Enqueue(new Popup(fish));
		}
	}

	public IEnumerator CreateLevelupPopup(LevelReward reward, bool late = false){
		yield return new WaitForEndOfFrame();
		if(!popupOpen){
			popupQueue.Enqueue(new Popup(reward));
		}else{
			popupQueue.Enqueue(new Popup(reward));
		}
	}

	public void FakeFishCatch(){
		StartCoroutine(CreateFishPopup(FindObjectOfType<FishDatabase>().fish[0]));
	}

	IEnumerator FadeOut(){
		Color old = xpText.color;
		old.a = 1;
		xpText.color = old;
		for (int i = 0; i < 100; i++) {
			old.a -= 0.01f;
			xpText.color = old;
			yield return new WaitForEndOfFrame();
		}
	}

	public void ClosePopup(){
		StartCoroutine(ClosePopupCoroutine());
	}

	public IEnumerator ClosePopupCoroutine(){

		float lerpTimer = 1;
		for (int i = 50; i > 0; i--) {
			uiPopup.transform.localScale = new Vector3(lerpTimer,lerpTimer,1);
			lerpTimer -= 0.02f;
			yield return new WaitForEndOfFrame();
		}
		popupOpen = false;
		currentPopup.SetActive(false);
		FindObjectOfType<Cast>().canCast = true;
		if(popupQueue.Count >= 2){
			popupQueue.Dequeue();
			print(popupQueue.Count);
			if(popupQueue.Peek().type == Popup.popupType.fish){
				StartCoroutine(CreateFishPopup(popupQueue.Peek().fish,true));
			}else{
				CreateLevelupPopup(popupQueue.Peek().levelReward);
			}
			popupOpen = true;
		}else if(popupQueue.Count == 1){
			popupQueue.Dequeue();
		}
	}



	public float Round(float value, int digits) {
		
		float mult = Mathf.Pow(10.0f, (float)digits);
		return Mathf.Round(value * mult) / mult;
	}

}

public class Popup {
	public enum popupType{
		fish,
		levelUp
	}
	public popupType type;
	public Fish fish;
	public LevelReward levelReward;

	public Popup(Fish _fish){
		type = popupType.fish;
		fish = _fish;
		levelReward = null;
	}
	public Popup(LevelReward _levelReward){
		type = popupType.levelUp;
		levelReward = _levelReward;
		fish = null;
	}
}