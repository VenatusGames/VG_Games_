using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PopupController : MonoBehaviour{

	public GameObject uiPopup;
	public GameObject fishPopupObject;
	public Text popupFishName,popupFishWeight,popupFishPrice;
	public Text xpText;
	public Image popupBorder;
	private GameObject currentFish;

	void Update(){
		if(uiPopup.activeInHierarchy)
			fishPopupObject.transform.Rotate(new Vector3(0,2,0) * Time.deltaTime * 20);
	}

	public void CreatePopup(Fish fish){
		uiPopup.SetActive(true);
		fishPopupObject.transform.eulerAngles = new Vector3(0,0,0);
		Destroy(currentFish);
		currentFish = (GameObject)Instantiate(fish.fishPrefab,fishPopupObject.transform.position,fish.fishPrefab.transform.rotation,fishPopupObject.transform);
		popupFishName.text = fish.fishName;
		popupBorder.color = fish.rarityColour;
		float weight = Round(Random.Range(fish.minWeight,fish.maxWeight),2);
		float weightDiffernce = (weight-fish.baseWeight);
		print(weightDiffernce.ToString());
		int price = Mathf.RoundToInt(fish.basePrice + (weightDiffernce));
		popupFishWeight.text = "Weight: "+weight.ToString()+ "Kg";
		popupFishPrice.text = "Price: " + price.ToString() + "g";
		FindObjectOfType<Inventory>().money += price;
		FindObjectOfType<FishPedia>().fishList.fishList.Add(new FishpediaFish(fish.fishName,weight));
		StartCoroutine(FindObjectOfType<ExperienceSystem>().AddXP((int)(fish.baseXP + weightDiffernce)));
		xpText.text = "+"+(int)(fish.baseXP + weightDiffernce) + "xp";
		StartCoroutine(FadeOut());
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
		uiPopup.SetActive(false);
		FindObjectOfType<Cast>().canCast = true;
	}
	public float Round(float value, int digits) {
		
		float mult = Mathf.Pow(10.0f, (float)digits);
		return Mathf.Round(value * mult) / mult;
	}
}
