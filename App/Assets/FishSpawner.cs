using UnityEngine;
using System.Collections;

public class FishSpawner : MonoBehaviour {
	public int maxFish;
	public int curFish;

	public GameObject fishPrefab;
	void Start(){
		SpawnLoop();
	}

	void SpawnLoop(){
		if(curFish < maxFish && Random.Range(0,2) == 1){
			Vector3 pos = new Vector3(Random.Range(-2f,4f),1.15f,Random.Range(2f,9f));
			Instantiate(fishPrefab,pos,fishPrefab.transform.rotation);
			curFish++;
		}
		Invoke("SpawnLoop",2);
	}
}
