using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FishPediaListing : MonoBehaviour {
	public bool isCaught;
	public Text textBox;
	public FishpediaFish fish;

	public void UpdateListing(float weight, float length){
		if(isCaught){
			fish.weight = weight;
			fish.length = length;
			textBox.text = "Name: "+fish.fishName+" | Best Weight: "+fish.weight+" | Best Length: "+fish.length;
		}else{
			textBox.text = "Name: ??? | Best Weight: ??? | Best Length: ???";
		}
	}
}
