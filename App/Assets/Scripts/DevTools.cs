using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class DevTools : MonoBehaviour {


	public InputField text;
	public bool isOpen;
	public GameObject menuObj;
	public void upbug(){
		StartCoroutine(UploadBug());
	}

	public IEnumerator UploadBug(){
		WWWForm form = new WWWForm();
		form.AddField("body",text.text);
		WWW req = new WWW("http://localhost/bug",form);
		yield return req;
		if(req.error.Length > 0){
			print(req.error);
		}else{

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




//	public GameObject menuObj;
//	public bool isOpen;
//	public Dropdown dropDown;
//
//	public InputField missionText;
//	public InputField weight,rarity,money,xp;
//	public InputField repeats;
//
//	void Start(){
//		Invoke("LateStart",0.1f);
//	}
//
//	void LateStart(){
//		List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
//		options.Add(new Dropdown.OptionData("None"));
//		foreach (var item in FindObjectOfType<FishDatabase>().fish) {
//			options.Add(new Dropdown.OptionData(item.fishName));
//		}
//		dropDown.options = options;
//	}
//

//	public void OpenFile(){
//		System.Diagnostics.Process.Start(Application.persistentDataPath);
//	}
//
//	public void AddMission(){
//		float weightNum = float.Parse(weight.text);
//		int id = dropDown.value;
//		int rarityNum = int.Parse(rarity.text);
//		string missionT = missionText.text;
//		int rewardMoney = int.Parse(money.text);
//		int rewardXp = int.Parse(xp.text);
//		int repeat = int.Parse(repeats.text);
//
//		Task task = new Task(id,rarityNum,weightNum,repeat);
//		Reward reward = new Reward(rewardXp,rewardMoney);
//		FindObjectOfType<MissionDatabase>().missions.Add(new Mission(task,reward,missionT));
//		FindObjectOfType<MissionDatabase>().Save();
//	}
}
