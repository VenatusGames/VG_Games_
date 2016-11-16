using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReelMovement : MonoBehaviour {
	public float radius;
	public RectTransform circle;
	public RectTransform stick;

	Vector2 oldPos;
	Cast cast;
	public bool isHolding;
	void Start(){
		cast = FindObjectOfType<Cast>();
		oldPos = transform.position;
	}

	void Update(){
		if(isHolding){
			Vector3 diff = Input.mousePosition - stick.position;
			diff.Normalize();
			float rot_z = Mathf.Atan2(diff.y,diff.x) * Mathf.Rad2Deg;
			stick.rotation = Quaternion.Euler(0f,0f,rot_z-180);
			cast.isReeling = true;
			cast.reelSpeed = Vector2.Distance(oldPos,transform.position);
			oldPos = transform.position;
		}else{
			cast.isReeling = false;
			cast.reelSpeed = 0f;
		}
	}

	public void pointerDown(){
		isHolding = true;
	}
	public void pointerUp(){
		isHolding = false;
	}
}
//Vector3 diff = circle.position - GetComponent<RectTransform>().position;
//diff.Normalize();
//float rot_z = Mathf.Atan2(diff.y,diff.x) * Mathf.Rad2Deg;
//GetComponent<RectTransform>().rotation = Quaternion.Euler(0f,0f,rot_z - 90);
//
//Vector3 diff2 = GetComponent<RectTransform>().position - stick.position;
//diff2.Normalize();
//float rot_z2 = Mathf.Atan2(diff2.y,diff2.x) * Mathf.Rad2Deg;
//stick.rotation = Quaternion.Euler(0f,0f,rot_z2 - 180);
//
//Vector2 newPos = (Vector2)Input.mousePosition - (Vector2)circle.position;
//newPos.Normalize();
//newPos *= radius;
//GetComponent<RectTransform>().anchoredPosition = newPos;