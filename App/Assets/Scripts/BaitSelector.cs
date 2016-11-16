using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
public class BaitSelector : MonoBehaviour {
	public RectTransform center;
	public List<RectTransform> baitRects;
	public RectTransform contentRect;
	public GameObject baitPrefab;
	public BaitIdentifier currentBait;
	private Inventory playerInv;
	private Cast playerCast;
	public Sprite defaultBaitImage;

	private bool canSetBait = false;

	void Start(){
		playerCast = FindObjectOfType<Cast>();
		playerInv = FindObjectOfType<Inventory>();
		Invoke("PopulateSlider",0.01f);
	}

	void Update(){
		if(Input.GetMouseButtonUp(0)){
			CenterSlider();
		}
		if(Input.mouseScrollDelta != Vector2.zero){
			CenterSlider();
		}
		if(canSetBait){
			if(currentBait.baitID == -1){
				playerCast.currentBait = new BaitType();
				playerCast.currentBait.image = defaultBaitImage;

			}else{
				playerCast.currentBait = playerInv.baits.Find(x => x.ID == currentBait.baitID);
			}
		}
	}

	public void PopulateSlider(){
		canSetBait = true;
		int i = 0;
		foreach (var item in baitRects) {
			Destroy(item.gameObject);
		}
		baitRects = new List<RectTransform>();
		GameObject temp = (GameObject)Instantiate(baitPrefab,contentRect.transform);
		temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(i*40,0);
		baitRects.Add(temp.GetComponent<RectTransform>());
		temp.GetComponent<BaitIdentifier>().baitID = -1;
		temp.GetComponent<BaitIdentifier>().index = i;
		i++;
		List<BaitType> sortedBaits = FindObjectOfType<Inventory>().baits.FindAll(x => x.maxMapID == FindObjectOfType<MapController>().currentMap.ID).OrderBy(x => x.ID).ToList();
		foreach (var item in sortedBaits) {
			GameObject cur = (GameObject)Instantiate(baitPrefab,contentRect.transform);
			cur.GetComponent<RectTransform>().anchoredPosition = new Vector2(i*40,0);
			baitRects.Add(cur.GetComponent<RectTransform>());
			cur.GetComponent<BaitIdentifier>().baitID = item.ID;
			cur.GetComponent<BaitIdentifier>().index = i;
			i++;
		}
		if(currentBait == null){
			currentBait = temp.GetComponent<BaitIdentifier>();
		}
	}

	public void CenterSlider(){
		
		float dist = Mathf.Infinity;
		RectTransform final = null;
		foreach (var item in baitRects) {
			if(Vector2.Distance(item.transform.position,center.transform.position) < dist){
				dist = Vector2.Distance(item.transform.position,center.transform.position);
				final = item;
			}

		}
		currentBait = final.GetComponent<BaitIdentifier>();
		int id = final.GetComponent<BaitIdentifier>().index;
		contentRect.offsetMax = new Vector2(200-(id*40),contentRect.offsetMax.y);
		contentRect.offsetMin = new Vector2(-200-(id*40),contentRect.offsetMin.y);
	}
}
