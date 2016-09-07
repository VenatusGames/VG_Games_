using UnityEngine;
using System.Collections;

[System.Serializable]
public class FishpediaFish {

	public string fishName;
	public float weight,length;

	public FishpediaFish(string _fishName, float _weight, float _length){
		fishName = _fishName;
		weight = _weight;
		length = _length;
	}
}
