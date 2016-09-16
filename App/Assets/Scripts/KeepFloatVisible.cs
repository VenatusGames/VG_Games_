using UnityEngine;
using System.Collections;

public class KeepFloatVisible : MonoBehaviour {
	public MeshRenderer rod;
	Cast cast;
	public Material transparent,opaque;
	Ray towardsPlayer;

	void Start(){
		rod = GameObject.FindGameObjectWithTag("Rod").GetComponent<MeshRenderer>();
		cast = FindObjectOfType<Cast>();
	}

	void Update(){
		towardsPlayer = new Ray(this.transform.position,(Camera.main.transform.position-transform.position));
		RaycastHit hit = new RaycastHit();
		if(Physics.Raycast(towardsPlayer, out hit)){
			if(hit.collider.gameObject.tag == "Rod" && !cast.reelStarted){
				rod.material = transparent;
			}else{
				rod.material = opaque;
			}
		}else{
			rod.material = opaque;
		}
	}

}
