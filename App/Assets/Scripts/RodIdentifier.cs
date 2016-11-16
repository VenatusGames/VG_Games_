using UnityEngine;
using System.Collections;

public class RodIdentifier : MonoBehaviour {
	public int id;

	void Start(){
		Material mat = new Material(Shader.Find("Mobile/Diffuse"));
		mat.SetTexture("_MainTex",FindObjectOfType<ItemDatabase>().rods[id].rodTexture);
		GetComponent<MeshRenderer>().material = mat;
	}
}
