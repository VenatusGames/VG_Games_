using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class ExperienceSystem : MonoBehaviour {
	public float currentXP;
	public int level;
	public List<int> maxXpArray;
	public int length;

	public Text levelText;
	public Scrollbar xpSlider;

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
		Save();
		maxXpArray.Add(50);
		maxXpArray.Add(200);
		maxXpArray.Add(500);
		for (int i = 2; i < length-1; i++) {
			int dif = maxXpArray[i] - maxXpArray[i-1];
			maxXpArray.Add((Mathf.RoundToInt(dif + (10 * i) + maxXpArray[i])/10)*10);
		}
	}

	public void ButtonAddXp(int xp){
		StartCoroutine(AddXP(xp));
	}

	public IEnumerator AddXP(int xp){
		float start = currentXP;
		float final = currentXP + xp;
		float lerpTimer = 0;
		for (int i = 0; i < 100; i++) {
			currentXP = Mathf.Lerp(start,final,lerpTimer);
			if(currentXP >= maxXpArray[level-1]){
				final = (start + xp) - maxXpArray[level-1];
				start = 0;
				level ++;
			}
			lerpTimer += 0.01f;
			yield return new WaitForEndOfFrame();
		}
		if(currentXP >= maxXpArray[level-1]){
			currentXP = currentXP - maxXpArray[level-1] ;
			level ++;
		}
	}

	void Update(){
		levelText.text = level.ToString();
		xpSlider.size = ((float)currentXP / (float)maxXpArray[level-1]);
	}

	void Save(){
		ExperienceSave save = new ExperienceSave(currentXP,level);
		string jsonSaveString = JsonUtility.ToJson(save,true);
		System.IO.File.WriteAllText(Application.persistentDataPath + @"\xp.json",jsonSaveString);
		Invoke("Save",5);
	}
}
