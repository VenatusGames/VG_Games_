using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LevelReward {
	public List<BaitFinder> baitRewards;
	public RodFinder rodReward;
	public BobberFinder bobberReward;
	public int xpReward;
	public int moneyReward;
}

