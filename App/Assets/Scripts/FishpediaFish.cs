using UnityEngine;
using System.Collections;

[System.Serializable]
public class FishpediaFish {

	public string fishName;
	public float weight;

	public FishpediaFish(string _fishName, float _weight){
		fishName = _fishName;
		weight = _weight;
	}
}
