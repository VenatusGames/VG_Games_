using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DetectHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
	public bool isDown;

	public void OnPointerEnter(PointerEventData eventData){
		isDown = true;
	}
	public void OnPointerExit(PointerEventData eventData){
		isDown = false;
	}
}
