using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System.Timers;
public class MissionUI : MonoBehaviour {
	public List<Mission> missions = new List<Mission>();
	public Mission trackedMission;
	MissionController missionControl;
	public List<Toggle> toggles;
	public bool isOpen;
	public GameObject menuObj;
	public bool dropDownisOpen = true;
	public GameObject arrow;
	public AnimationCurve popoutCurve,stampCurve;

	public List<Text> missionTexts;
	public List<GameObject> missionSliders;
	public List<Image> missionStamps;
	public List<Text> missionTimers;

	void Start(){
		missionControl = FindObjectOfType<MissionController>();
		missions = missionControl.curMissions;
		foreach (var item in missions) {
			if(item.task.completed){
				int index = missions.IndexOf(item);
				GameObject curBox = missionTexts[index].transform.parent.gameObject;
				curBox.transform.localPosition = new Vector3(-130,curBox.transform.localPosition.y,0);
				missionTimers[index].color = new Color(1,1,1,1);
			}
		}
	}
	void Update(){
		missions = missionControl.curMissions;
		foreach (var item in missionTimers) {
			
			System.TimeSpan timeTillNewMissions = FindObjectOfType<MissionController>().timeToReach.Subtract(System.DateTime.Now);
			item.text = timeTillNewMissions.Hours+":"+timeTillNewMissions.Minutes+":"+timeTillNewMissions.Seconds;

		}

		for (int i = 0; i < 3; i++) {
			if(toggles[i].isOn){
				missions[i].isTracking = true;
			}else{
				missions[i].isTracking = false;
			}
			toggles[i].transform.parent.FindChild("MissionText").GetComponent<Text>().text = missions[i].missionText;
			if(missions[i].task.repeats < 2){
				if(!missions[i].task.completed)
					toggles[i].transform.parent.FindChild("Count").GetComponent<Text>().text = "Not Yet Complete";
				else
					toggles[i].transform.parent.FindChild("Count").GetComponent<Text>().text = "Complete";
			}else if(!missions[i].task.completed){
				toggles[i].transform.parent.FindChild("Count").GetComponent<Text>().text = missions[i].task.timesCompleted+"/"+missions[i].task.repeats;
			}else{
				toggles[i].transform.parent.FindChild("Count").GetComponent<Text>().text = "Complete";
			}
		}

		for (int i = 0; i < missionTexts.Count; i++) {
			missionTexts[i].text = missions[i].missionText;
			float percent = 0;

			if(missions[i].task.timesCompleted == 0 && missions[i].task.repeats > 1){
				
			}else if(missions[i].task.repeats == 1){
				if(missions[i].task.completed){
					percent = 1f;
				}
			}else{
				percent = (float)missions[i].task.timesCompleted/(float)missions[i].task.repeats;
			}
			missionSliders[i].transform.localPosition = Vector3.Lerp(new Vector3(-94,missionSliders[i].transform.localPosition.y,0),new Vector3(0,missionSliders[i].transform.localPosition.y,0),percent);
		}

	}
	public void Toggle(){
		isOpen = !isOpen;
		menuObj.SetActive(!menuObj.activeSelf);
		if(isOpen){
			FindObjectOfType<Cast>().canCast = false;
		}else if(FindObjectOfType<Cast>().canCast == false && FindObjectOfType<Cast>().hasCasted == false){
			FindObjectOfType<Cast>().canCast = true;
		}
	}

	public void ToggleDrop(){
		StartCoroutine(ToggleDropdown());
	}

	IEnumerator ToggleDropdown(){
		RectTransform rect = GetComponent<RectTransform>();
		dropDownisOpen = !dropDownisOpen;
		float lerpTimer = 0f;

		while (lerpTimer <= 0.5f) {
			if(dropDownisOpen){
				arrow.transform.eulerAngles = Vector3.Lerp(new Vector3(0,180,-90),new Vector3(0,0,-90),popoutCurve.Evaluate(lerpTimer/0.5f));
				rect.transform.position = Vector3.Lerp(new Vector3(-130,rect.transform.position.y,0),new Vector3(0,rect.transform.position.y,0),popoutCurve.Evaluate(lerpTimer/0.5f));
				lerpTimer += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}else{
				arrow.transform.eulerAngles = Vector3.Lerp(new Vector3(0,0,-90),new Vector3(0,180,-90),popoutCurve.Evaluate(lerpTimer/0.5f));
				rect.transform.position = Vector3.Lerp(new Vector3(0,rect.transform.position.y,0),new Vector3(-130,rect.transform.position.y,0),popoutCurve.Evaluate(lerpTimer/0.5f));
				lerpTimer += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
		}
	}

	public void FakeSlider(bool a){
		if(a)
			StartCoroutine(StampMission(0));
		else
			StartCoroutine(ReverseStamp(0));
	}

	public IEnumerator StampMission(int i){
		if(dropDownisOpen){
			float lerpTimer = 0f;

			while (lerpTimer <= 0.5f) {
				missionStamps[i].color = new Color(1,1,1,Mathf.Lerp(0,1,stampCurve.Evaluate(lerpTimer/0.5f)));
				lerpTimer += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			yield return new WaitForSeconds(0.5f);
			lerpTimer = 0f;
			GameObject missionBox = missionTexts[i].transform.parent.gameObject;
			while (lerpTimer <= 0.5f) {
				missionBox.transform.localPosition = Vector3.Lerp(new Vector3(0,missionBox.transform.localPosition.y,0),new Vector3(-130,missionBox.transform.localPosition.y,0),popoutCurve.Evaluate(lerpTimer/0.5f));
				lerpTimer += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			lerpTimer = 0f;
			while (lerpTimer <= 0.5f) {
				missionTimers[i].color = new Color(1,1,1,Mathf.Lerp(0,1,stampCurve.Evaluate(lerpTimer/0.5f)));
				lerpTimer += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}

		}else{
			yield return new WaitForEndOfFrame();
			StartCoroutine(StampMission(i));
		}
	}
	public IEnumerator ReverseStamp(int i){
		if(dropDownisOpen){
			float lerpTimer = 0f;

			while (lerpTimer <= 0.5f) {
				missionTimers[i].color = new Color(1,1,1,Mathf.Lerp(1,0,stampCurve.Evaluate(lerpTimer/0.5f)));
				lerpTimer += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			missionStamps[i].color = new Color(1,1,1,0);
			lerpTimer = 0f;
			GameObject missionBox = missionTexts[i].transform.parent.gameObject;
			while (lerpTimer <= 0.5f) {
				missionBox.transform.localPosition = Vector3.Lerp(new Vector3(-130,missionBox.transform.localPosition.y,0),new Vector3(0,missionBox.transform.localPosition.y,0),popoutCurve.Evaluate(lerpTimer/0.5f));
				lerpTimer += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}




		}else{
			yield return new WaitForEndOfFrame();
			StartCoroutine(ReverseStamp(i));
		}
	}
}
