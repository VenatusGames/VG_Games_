using UnityEngine;
using System.Collections;

public class KeepFloatVisible : MonoBehaviour {
	public MeshRenderer rod;
	Ray towardsPlayer;

	void Start(){
		rod = GameObject.FindGameObjectWithTag("Rod").GetComponent<MeshRenderer>();
	}

	void Update(){
		towardsPlayer = new Ray(this.transform.position,(Camera.main.transform.position-transform.position));
		RaycastHit hit = new RaycastHit();
		if(Physics.Raycast(towardsPlayer, out hit)){
			if(hit.collider.gameObject.tag == "Rod"){
				Color old = rod.material.color;
				old.a = 0.5f;
				rod.material.color = old;
			}else{
				Color old = rod.material.color;
				old.a = 1f;
				rod.material.color = old;
			}
		}else{
			Color old = rod.material.color;
			old.a = 1f;
			rod.material.color = old;
		}
	}

}
