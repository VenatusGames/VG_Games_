using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
public class BaitIdentifier : MonoBehaviour {
	public int baitID;
	public int index;
	Inventory inv;
	Text text;
	public Image baitImage;
	void Start(){
		inv = FindObjectOfType<Inventory>();
		text = GetComponentInChildren<Text>();
	}
	void Update(){
		if(baitID == -1){
			text.text = "inf";
		}else{
			text.text = inv.baits.Find(x => x.ID == baitID).amount.ToString();
			baitImage.sprite = inv.baits.Find(x => x.ID == baitID).image;
		}
	}
}
