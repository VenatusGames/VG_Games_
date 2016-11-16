using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class ExperienceSystem : MonoBehaviour {
	public float currentXP;
	public int level;
	public List<int> maxXpArray;
	public List<LevelReward> levelRewards;
	public int length;

	public Text levelText;
	public RectTransform xpSlider;

	public float xpToReach;
	void Start(){
		ExperienceSave loadSave = new ExperienceSave(0,0);
		if(System.IO.File.Exists(Application.persistentDataPath + @"\xp.json")){
			string jsonSaveString = System.IO.File.ReadAllText(Application.persistentDataPath + @"\xp.json");
			JsonUtility.FromJsonOverwrite(jsonSaveString,loadSave);
		}else{
			System.IO.File.Create(Application.persistentDataPath + @"\xp.json");
		}
		currentXP = loadSave.currentXP;
		level = loadSave.level;
		if(level == 0)
			level = 1;
		xpToReach = currentXP;
		Save();
		maxXpArray.Add(50);
		maxXpArray.Add(200);
		maxXpArray.Add(500);
		for (int i = 2; i < length-1; i++) {
			int dif = maxXpArray[i] - maxXpArray[i-1];
			maxXpArray.Add((Mathf.RoundToInt(dif + (10 * i) + maxXpArray[i])/10)*10);
		}
	}

	public void AddXP(int xp){
		xpToReach += xp;
		StartCoroutine(LerpAddXP(xp));
	}

	public IEnumerator LerpAddXP(int xp){
		float start = currentXP;
		float final = xpToReach;
		float lerpTimer = 0;
		for (int i = 0; i < 100; i++) {
			lerpTimer += 0.01f;
			if(currentXP >= maxXpArray[level-1]){
				final = (start + xp) - maxXpArray[level-1];
				start = 0;
				LevelUp();
			}
			currentXP = Mathf.RoundToInt(Mathf.Lerp(start,final,lerpTimer));
			yield return new WaitForEndOfFrame();
		}

		if(currentXP >= maxXpArray[level-1]){
			LevelUp();
		};
	}

	void LevelUp(){
		currentXP = 0;
		xpToReach = xpToReach - maxXpArray[level-1];
		level++;
		if(level % 5 == 0){
			print(level/5);
		}
	}

	void Update(){
		levelText.text = level.ToString();
		// -159 lowest 0 highest
		float percent = (float)currentXP / (float)maxXpArray[level-1];
		xpSlider.localPosition = Vector3.Lerp(new Vector3(-158.95f,0,0),new Vector3(0,0,0),percent);

	}

	void Save(){
		ExperienceSave save = new ExperienceSave(currentXP,level);
		string jsonSaveString = JsonUtility.ToJson(save,true);
		System.IO.File.WriteAllText(Application.persistentDataPath + @"\xp.json",jsonSaveString);
		Invoke("Save",5);
	}
}
