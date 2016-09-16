using UnityEngine;
using System.Collections;

public class WaterMovement : MonoBehaviour {
	public Vector2 targetPos;
	public Vector2 oldTargetPos;
	public MeshRenderer rend;
	void Start(){
		UpdateSpeed();
	}

	void Update () {
		oldTargetPos = Vector2.MoveTowards(oldTargetPos,targetPos,0.05f);
		rend.material.SetTextureOffset("_MainTex",Vector2.MoveTowards(rend.material.mainTextureOffset,oldTargetPos,0.00025f));
		if(oldTargetPos == targetPos)
			UpdateSpeed();
	}
	void UpdateSpeed(){
		oldTargetPos = targetPos;
		targetPos = new Vector2(Random.Range(-25f,25f),Random.Range(-25f,25));
	}
	void OnDrawGizmos(){
		Gizmos.DrawCube(new Vector3(targetPos.x,0,targetPos.y),Vector3.one);
		Gizmos.DrawCube(new Vector3(oldTargetPos.x,0,oldTargetPos.y),Vector3.one);
		Gizmos.DrawLine(new Vector3(targetPos.x,0,targetPos.y),new Vector3(oldTargetPos.x,0,oldTargetPos.y));
	}
}
