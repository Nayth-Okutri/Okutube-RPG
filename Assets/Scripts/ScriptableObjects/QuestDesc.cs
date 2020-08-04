using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct SocialStatRequirement
{
    public SocialStat stat;
    public int statAmount;
}
[System.Serializable]
public struct ItemRequirement
{
    public Item item;
    public int itemCount;
}

[System.Serializable]
public class TriggerRequirement
{
    public ItemRequirement[] itemRequirement;
    public SocialStatRequirement[] socialStatRequirement;
}

[CreateAssetMenu]
public class QuestDesc : GameSocialEvent
{
    //public string description;
    public int QuestId;
    public TriggerRequirement completionRequirements;
    public DialogConversation QuestInprogressConversation;
    public DialogConversation QuestCompletedConversation;
    public bool Completed;

    private void OnEnable()
    {
        Completed = false;
    }




    public string GenerateCompletionDescription()
    {
        string _Result="";
        if (completionRequirements == null) _Result = "Il n'y a pas de condition de complétion pour la quête : " + this.fullname;
        else
        {
            foreach (var i in completionRequirements.itemRequirement)
            {
                if (_Result == "") _Result += "Items nécessaires à la quête " + System.Environment.NewLine +"item : " + i.itemCount + " " + i.item.name;
                else _Result += System.Environment.NewLine + "Item : " + i.itemCount + " " + i.item.name;
            }
            foreach (var i in completionRequirements.socialStatRequirement)
            {
                if (_Result == "") _Result += "Stats nécessaires à la quête " + System.Environment.NewLine + "stat : " + i.statAmount + " " + i.stat.name;
                else _Result += System.Environment.NewLine + "Stat : " + i.statAmount + " " + i.stat.name;
            }
          
        }
            return _Result;
    }
     
}
