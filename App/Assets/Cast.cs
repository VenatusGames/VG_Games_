using UnityEngine;
using System.Collections;

public class Cast : MonoBehaviour {

	public GameObject floatPrefab;
	public Transform instObj;
	public Vector3 beginPos,endPos;

	public int maxPower;

	public bool canCast;
	public bool castStarted;
	private float startTime;
	public float lastSpeed;
	public float angle;
	void Update(){
		if(Input.touches.Length > 1 || Input.touches.Length < 1) return;

		switch (Input.touches[0].phase) {
			
		case TouchPhase.Began: 
			beginPos = Input.touches[0].position;
			startTime = Time.time;
			castStarted = true;
			break;
		case TouchPhase.Ended:
				StartCast();
			break;

		default:
			break;
		}
	}

	void StartCast(){
		if(castStarted){
			endPos = Input.touches[0].position;
			if(endPos.y > beginPos.y){
				float timeToRelease = Time.time - startTime;
				lastSpeed = Mathf.Clamp(Vector3.Distance(beginPos,endPos) / timeToRelease,0,maxPower);
				lastSpeed = (lastSpeed / maxPower) * 100;
				Vector2 swipe = beginPos - endPos;
				angle = Vector2.Angle(Vector2.right,swipe);
			}
		}
		beginPos = Vector3.zero;
		endPos = Vector3.zero;
		castStarted = false;
	}

	void OnGUI(){
		GUI.Label(new Rect(0,0,200,200),beginPos.ToString());
		GUI.Label(new Rect(0,20,200,200),endPos.ToString());
		GUI.Label(new Rect(0,40,200,200),lastSpeed.ToString());
		GUI.Label(new Rect(0,60,200,200),startTime.ToString());
		GUI.Label(new Rect(0,80,200,200),angle.ToString());
	}
}
