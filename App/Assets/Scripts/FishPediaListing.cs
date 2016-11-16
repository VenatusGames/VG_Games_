using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
public class FishPediaListing : MonoBehaviour {
	public bool isCaught;
	public int fishID;

	public GameObject fishPosition;
	private GameObject spawnedFish;

	public void UpdateListing(){
		if(spawnedFish != null)
			Destroy(spawnedFish);
		FishList fish = FindObjectOfType<FishPedia>().fishList;
		transform.FindChild("Border").GetComponent<Image>().color = FindObjectOfType<FishDatabase>().fish[fishID].rarityColour;
		GameObject child = transform.FindChild("FishPosition").gameObject;
		spawnedFish = (GameObject)Instantiate(FindObjectOfType<FishDatabase>().fish[fishID].fishPrefab,child.GetComponent<RectTransform>().position,transform.rotation,child.transform);
		spawnedFish.GetComponentInChildren<MeshRenderer>().gameObject.layer = LayerMask.NameToLayer("UI");
		spawnedFish.transform.localScale = new Vector3(spawnedFish.transform.localScale.x * 50f,spawnedFish.transform.localScale.y * 50f,spawnedFish.transform.localScale.z * 50f);
		spawnedFish.transform.localPosition = new Vector3(0,0,-40);

		if(!fish.fishList.Exists(x => x.fishName == FindObjectOfType<FishDatabase>().fish[fishID].fishName)){
			Material blackMat = (Material)Resources.Load("blackMat");
			spawnedFish.GetComponentInChildren<MeshRenderer>().material = blackMat;
		}
	}
}