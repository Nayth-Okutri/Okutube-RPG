using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct BonusStat
{
    public SocialStat stat;
    public int BonusAmount;
}
[System.Serializable]
public struct GameProgressionBonus 
{
    //Add loot luck
    public BonusStat bonusElement;
    public int bonusLootRate;
}
