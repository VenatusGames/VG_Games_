using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RenderLine : MonoBehaviour {

	public GameObject lure;
	LineRenderer render;
	public float drop,smoothing;
	public List<Transform> rodRingTransforms;
	void Start(){
		render = GetComponent<LineRenderer>();
	}

	void Update(){
		if(lure){
			drop = Mathf.Clamp(Vector3.Distance(lure.transform.position,transform.position)/10,0,0.5f);
			List<Vector3> positions = new List<Vector3>();
			float x = lure.transform.position.x;
			float y = lure.transform.position.y;
			float z = lure.transform.position.z;
			positions.Add(transform.position);
			Vector3 middlePos = Vector3.Lerp(transform.position,new Vector3(x,y,z),0.50f);
			positions.Add(new Vector3(middlePos.x,middlePos.y-drop,middlePos.z));

			positions.Add(new Vector3(x,y+0.2f,z));
			List<Vector3> positionsArray = Curver.MakeSmoothCurve(positions,smoothing);
			if(rodRingTransforms.Count > 0){
				foreach (var item in rodRingTransforms) {
					positionsArray.Insert(0,item.transform.position);
				}
			}
			render.SetVertexCount(positionsArray.Count);
			render.SetPositions(positionsArray.ToArray());
		}else{
			if(rodRingTransforms.Count > 0){
				render.SetVertexCount(rodRingTransforms.Count);
				List<Vector3> positions = new List<Vector3>();
				foreach (var item in rodRingTransforms) {
					positions.Add(item.transform.position);
				}
				render.SetPositions(positions.ToArray());
			}else{
				render.SetVertexCount(2);
				render.SetPositions(new Vector3[2]{new Vector3(0,0,0),new Vector3(0,0,0)});
			}
		}

	}
}