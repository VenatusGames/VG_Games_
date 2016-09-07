using UnityEngine;
using System.Collections;

public class Hotspot : MonoBehaviour {
	public bool isActive;
	public bool moving;
	// Use this for initialization
	void Start () {
		UpdateHotSpotPos();
	}


	public void UpdateHotSpotPos(){
		if(isActive){
			isActive = false;
			GetComponent<ParticleSystem>().Stop();
			GetComponent<CapsuleCollider>().enabled = false;
		}else if(!moving){
			Vector3 pos = new Vector3(Random.Range(-2.86f,3f),1.15f,Random.Range(5f,8f));
			transform.position = pos;
			moving = true;
		}else if(moving){
			isActive = true;
			moving = false;
			GetComponent<ParticleSystem>().Play();
			GetComponent<CapsuleCollider>().enabled = true;
		}
		Invoke("UpdateHotSpotPos",Random.Range(5f,15f));
	}

}
