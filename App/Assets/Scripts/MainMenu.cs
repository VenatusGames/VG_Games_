using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public bool isOpen = false;
	public GameObject menuObject;
	public GameObject menuButton;
	public void ToggleMainMenu(){
		isOpen = !isOpen;
		menuObject.SetActive(isOpen);
		if(isOpen){
			FindObjectOfType<Cast>().canCast = false;
		}else if(FindObjectOfType<Cast>().canCast == false && FindObjectOfType<Cast>().hasCasted == false){
			FindObjectOfType<Cast>().canCast = true;
		}
	}
	void Update(){
		if(isOpen && !FindObjectOfType<Cast>().hasCasted)
			FindObjectOfType<Cast>().canCast = false;
	}
}
