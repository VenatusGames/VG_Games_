using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FishPediaListing : MonoBehaviour {
	public bool isCaught;
	public FishpediaFish fish;

	public void UpdateListing(float weight){
		if(isCaught){
			fish.weight = weight;
		}else{
			
		}
	}

	void Update(){
		if(GetComponentInChildren<DetectHover>().isDown){
			GetComponentInChildren<MeshRenderer>().transform.parent.transform.Rotate(0,1,0);
		}else{
			GetComponentInChildren<MeshRenderer>().transform.parent.transform.localEulerAngles = new Vector3(0,0,0);
		}
	}
}
