using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class MissionController : MonoBehaviour {

	public System.DateTime oldTime;
	public System.DateTime timeToReach = System.DateTime.Now;
	public List<Mission> curMissions = new List<Mission>();
	public List<Toggle> toggles;
	public List<Text> missionTexts;
	public GameObject menuObj;
	public bool isOpen;

	void Start () {
		bool addMissions = false;
		string timeString = PlayerPrefs.GetString("TimeString");
		if(timeString != ""){
			System.DateTime savedTime = new System.DateTime(System.Convert.ToInt64(timeString));
			System.DateTime newTime = System.DateTime.Now;
			System.TimeSpan timePassed = newTime.Subtract(savedTime);
			if(timePassed >= System.TimeSpan.FromHours(1)){
				oldTime = System.DateTime.Now;
				addMissions = true;
			}else{
				oldTime = savedTime;
			}
		}else{
			oldTime = System.DateTime.Now;
			addMissions = true;
		}

		if(addMissions){
			timeToReach = System.DateTime.Now.AddHours(1);
		}else{
			timeToReach = oldTime.AddHours(1);
		}
		SaveTime();
		if(addMissions){
			curMissions.Clear();
			for (int i = 0; i < 3; i++) {
				Mission missionToAdd = FindObjectOfType<MissionDatabase>().missions[Random.Range(0,FindObjectOfType<MissionDatabase>().missions.Count)];
				curMissions.Add(new Mission(missionToAdd.task,missionToAdd.reward,missionToAdd.missionText));
			}
			SaveMissions();
		}else{
			curMissions.Clear();
			LoadMissions();
		}
	}

	void Update(){
		System.DateTime savedTime = oldTime;
		System.DateTime newTime = System.DateTime.Now;
		System.TimeSpan timePassed = newTime.Subtract(savedTime);

		if(timePassed >= System.TimeSpan.FromHours(1)){
			oldTime = System.DateTime.Now;
			AddMissions();
			SaveTime();
		}
	}

	public float CheckFishForMissions(Fish fish,float fishWeight){
		float xpToAdd = 0;
		if(fish.isJunk)
			return 0f;
		foreach (Mission mission in curMissions) {
			
			bool satisfiesTask = true;

			if(mission.task.weight){
				if(fishWeight < mission.task.weightGoal){
					satisfiesTask = false;
				}
			}

			if(mission.task.rarity){
				if(fish.rarityLevel < mission.task.rarityGoal){
					satisfiesTask = false;
				}
			}

			if(mission.task.specificFish){
				if(fish.fishName != FindObjectOfType<FishDatabase>().fish[mission.task.fishGoalId].fishName){
					satisfiesTask = false;
				}
			}

			if(satisfiesTask == true){
				if(mission.task.repeats < 2){
					mission.task.completed = true;
				}else{
					mission.task.timesCompleted ++;
					if(mission.task.timesCompleted >= mission.task.repeats){
						mission.task.completed = true;
					}
				}
			}

			if(mission.task.completed && !mission.rewardClaimed){
				FindObjectOfType<Inventory>().money += mission.reward.moneyReward;
				xpToAdd += mission.reward.xpReward;
				StartCoroutine(FindObjectOfType<MissionUI>().StampMission(curMissions.IndexOf(mission)));

				mission.rewardClaimed = true;
			}
		}

		return xpToAdd;
	}

	public void SaveMissions(){
		
		MissionSave save = new MissionSave(this.curMissions);
		string jsonSaveString = JsonUtility.ToJson(save,true);
		System.IO.File.WriteAllText(Application.persistentDataPath + @"\missions.json",jsonSaveString);
		Invoke("SaveMissions",5);

	}

	public void LoadMissions(){
		
		MissionSave loadSave = new MissionSave(new List<Mission>());
		if(System.IO.File.Exists(Application.persistentDataPath + @"\missions.json")){
			string jsonSaveString = System.IO.File.ReadAllText(Application.persistentDataPath + @"\missions.json");
			JsonUtility.FromJsonOverwrite(jsonSaveString,loadSave);
		}else{
			System.IO.File.Create(Application.persistentDataPath + @"\missions.json");
		}
		curMissions = loadSave.missions;
		SaveMissions();

	}

	void AddMissions(){
		curMissions.Clear();
		oldTime = System.DateTime.Now;
		timeToReach = System.DateTime.Now.AddHours(1);
		for (int i = 0; i < 3; i++) {
			Mission missionToAdd = FindObjectOfType<MissionDatabase>().missions[Random.Range(0,FindObjectOfType<MissionDatabase>().missions.Count)];
			curMissions.Add(new Mission(missionToAdd.task,missionToAdd.reward,missionToAdd.missionText));
		}
		SaveMissions();
	}

	void SaveTime(){
		PlayerPrefs.SetString("TimeString",oldTime.Ticks.ToString());
		PlayerPrefs.Save();
		Invoke("SaveTime",5f);
	}
}

[System.Serializable]
public class Mission {
	public Task task;
	public Reward reward;
	public string missionText;
	public bool isTracking;
	public bool rewardClaimed;
	public Mission(Task t, Reward r, string mt){
		this.task = t;
		this.reward = r;
		this.missionText = mt;
	}
}

[System.Serializable]
public struct Task{
	public bool weight,rarity,specificFish;
	public float weightGoal;
	public int fishGoalId;
	public int rarityGoal;
	public int timesCompleted;
	public int repeats;
	public bool completed;

	public Task(int id, int rar, float w, int repeat){
		
		if((int)w == 0){
			weight = false;
			weightGoal = 0;
		}else{
			weight = true;
			weightGoal = w;
		}
		if(id == 0){
			specificFish = false;
			fishGoalId = 0;
		}else{
			specificFish = true;
			fishGoalId = id-1;
		}
		if(rar == 0){
			rarity = false;
			rarityGoal = 0;
		}else{
			rarity = true;
			rarityGoal = rar;
		}
		completed = false;
		timesCompleted = 0;
		repeats = repeat;

	}
}
[System.Serializable]
public class Reward{
	public int xpReward,moneyReward;

	public Reward(int x,int m){
		xpReward = x;
		moneyReward = m;
	}
}
