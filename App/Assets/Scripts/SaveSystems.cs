using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class ExperienceSave{
	public float currentXP;
	public int level;

	public ExperienceSave(float _a, int _b){
		currentXP = _a;
		level = _b;
	}
}
[System.Serializable]
public class FishDatabaseSave{
	public List<Fish> fish;

	public FishDatabaseSave(List<Fish> a){
		fish = a;
	}
}
