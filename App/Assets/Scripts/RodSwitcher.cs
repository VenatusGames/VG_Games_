using UnityEngine;
using System.Collections;
using System.Linq;
public class RodSwitcher : MonoBehaviour {
	public GameObject rodObj;
	public Transform instObj;

	void Start(){
		Invoke("LateStart",0.01f);
	}

	void LateStart(){
		
		if(FindObjectOfType<Inventory>().rods.Exists(x => x.isEquipped)){
			SwitchRod(FindObjectOfType<Inventory>().rods.Find(x => x.isEquipped));
		}else if(!FindObjectOfType<Inventory>().rods.Exists(x => x.ID == 8)){
			FindObjectOfType<Inventory>().rods.Add(FindObjectOfType<ItemDatabase>().rods[8]);
			FindObjectOfType<Inventory>().rods[0].isEquipped = true;
			SwitchRod(FindObjectOfType<ItemDatabase>().rods[8]);
		}else{
			SwitchRod(FindObjectOfType<ItemDatabase>().rods[8]);
		}
		if(FindObjectOfType<Inventory>().bobbers.Exists(x => x.isEquipped)){
			SwitchBobber(FindObjectOfType<Inventory>().bobbers.Find(x => x.isEquipped));
		}else if(!FindObjectOfType<Inventory>().bobbers.Exists(x => x.ID == 0)){
			FindObjectOfType<Inventory>().bobbers.Add(FindObjectOfType<ItemDatabase>().bobbers[0]);
			FindObjectOfType<Inventory>().bobbers[0].isEquipped = true;
			SwitchBobber(FindObjectOfType<ItemDatabase>().bobbers[0]);
		}else{
			SwitchBobber(FindObjectOfType<ItemDatabase>().bobbers[0]);
		}

	}

	public void SwitchRod(RodType rod){
		if(rodObj.GetComponentInChildren<RodIdentifier>().id == rod.ID){
			Destroy(rodObj.gameObject);
			GameObject newRod = (GameObject)Instantiate(FindObjectOfType<ItemDatabase>().rods[8].rod,instObj,false);
			int id = 8;
			FindObjectOfType<Inventory>().rods.Find(x => x.ID == id).isEquipped = true;
			foreach (var item in FindObjectOfType<Inventory>().rods.FindAll(x=>x.ID != id)) {
				item.isEquipped = false;
			}
			rodObj = newRod;
		}else{
			Destroy(rodObj.gameObject);
			GameObject newRod = (GameObject)Instantiate(FindObjectOfType<ItemDatabase>().rods[rod.ID].rod,instObj,false);
			int id = newRod.GetComponentInChildren<RodIdentifier>().id;
			FindObjectOfType<Inventory>().rods.Find(x => x.ID == id).isEquipped = true;
			foreach (var item in FindObjectOfType<Inventory>().rods.FindAll(x=>x.ID != rod.ID)) {
				item.isEquipped = false;
			}
			rodObj = newRod;
		}
	}
	public void SwitchBobber(BobberType bobber){
		if(FindObjectOfType<Cast>().floatPrefab.GetComponent<KeepFloatVisible>().bobberID == bobber.ID){

		}else{
			FindObjectOfType<Cast>().floatPrefab = bobber.bobber;
			FindObjectOfType<Inventory>().bobbers.Find(x => x.ID == bobber.ID).isEquipped = true;
			foreach (var item in FindObjectOfType<Inventory>().bobbers.FindAll(x => x.ID != bobber.ID)) {
				item.isEquipped = false;
			}
		}
	}
}
