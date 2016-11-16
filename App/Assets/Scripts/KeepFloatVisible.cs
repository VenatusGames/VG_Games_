using UnityEngine;
using System.Collections;

public class KeepFloatVisible : MonoBehaviour {
	public int bobberID;
	public GameObject rod;
	public Material transparent,opaque;
	Ray towardsPlayer;

	void Start(){
		transparent = new Material(Shader.Find("Legacy Shaders/Transparent/Diffuse"));
		transparent.color = new Color(1,1,1,0.2f);
		opaque = new Material(Shader.Find("Mobile/Diffuse"));
		rod = GameObject.FindGameObjectWithTag("Rod");
	}

	void Update(){
		towardsPlayer = new Ray(this.transform.position,(Camera.main.transform.position-transform.position));
		RaycastHit hit = new RaycastHit();
		if(Physics.Raycast(towardsPlayer, out hit)){
			if(hit.collider.gameObject.tag == "Rod"){
				transparent.mainTexture = FindObjectOfType<ItemDatabase>().rods[FindObjectOfType<RodIdentifier>().id].rodTexture;
				rod.GetComponent<MeshRenderer>().material = transparent;
			}else{
				opaque.mainTexture = FindObjectOfType<ItemDatabase>().rods[FindObjectOfType<RodIdentifier>().id].rodTexture;
				rod.GetComponent<MeshRenderer>().material = opaque;
			}
		}else{
			opaque.mainTexture = FindObjectOfType<ItemDatabase>().rods[FindObjectOfType<RodIdentifier>().id].rodTexture;
			rod.GetComponent<MeshRenderer>().material = opaque;
		}
	}
	void OnDestroy(){
		if(rod != null)
			rod.GetComponent<MeshRenderer>().material = opaque;
	}

}
