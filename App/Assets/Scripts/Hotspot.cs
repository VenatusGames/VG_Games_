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
			Vector3 pos = new Vector3(Random.Range(-6.62f,6.62f),0.92f,Random.Range(5f,11.5f));
			transform.position = pos;
			moving = true;
		}else if(moving){
			isActive = true;
			moving = false;
			GetComponent<ParticleSystem>().Play();
			GetComponent<CapsuleCollider>().enabled = true;
		}
		Invoke("UpdateHotSpotPos",Random.Range(10f,15f));
	}

}
