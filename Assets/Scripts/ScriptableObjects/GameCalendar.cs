using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct SocialStatReward
{
    public SocialStat stat;
    public int statAmount;
}
[System.Serializable]
public struct ItemReward
{
    public Item item;
    public int itemCount;
}

[System.Serializable]
public class GameReward
{
    public SideKick sideKick;
    public ItemReward[] itemRewards;
    public SocialStatReward[] statRewards;
}


[System.Serializable]
public class GameSocialEvent : ScriptableObject
{
    public string fullname;
    private bool IsActive;
    [TextArea(3, 5)]
    public string description;
    public string shortDescription;
    public GameReward Reward;
    public TriggerRequirement startingRequirements;

    public string GetStatRewardNames()
    {
        string _result = "";
        if ((Reward != null) && (Reward.statRewards.Length > 0))
            foreach (var i in Reward.statRewards)
                _result += i.statAmount + " " + i.stat.name + " ";
        return _result;

    }

    public string GetItemsRewardNames()
    {
        string _result = "";
        if ((Reward != null) && (Reward.itemRewards.Length > 0))
            foreach (var i in Reward.itemRewards)
                _result += i.itemCount + " " + i.item.name + " ";
        return _result;

    }
    public string GetRewardNames()
    {
        string _result = "";
        _result = GetStatRewardNames() + System.Environment.NewLine + GetItemsRewardNames();

        return _result;
    }

    public void GiveRewards()
    {
        foreach(var itemReward in Reward.itemRewards)
        {
            Inventory.Instance.AddInventoryItem(itemReward.item, itemReward.itemCount);

        }
        foreach (var statReward in Reward.statRewards)
        {
            ProgressionSystem.Instance.InscreaseSocialStat(statReward.stat.name, statReward.statAmount);
        }

    }


}




[System.Serializable]
public class GameEventInstance
{
    public GameSocialEvent gameEvent;
    public int Occurrence = 1;
    public float probability;
}
[CreateAssetMenu]
public class GameCalendar : ScriptableObject
{
    [SerializeField]
    public List<GameEventInstance> BaseSocialEvents;
    public Dictionary<int, List<GameSocialEvent>> ActivitiesSchedule;
    public int NumberOfDays;


    public void InitializeCalendar()
    {


    }
    // Start is called before the first frame update
    void OnEnable()
    {
        ActivitiesSchedule = new Dictionary<int, List<GameSocialEvent>>();
        foreach (GameEventInstance BaseGameEvent in BaseSocialEvents)
        {
            for (int i=0;i< BaseGameEvent.Occurrence;i++)
            { 
                int index = Random.Range(0, NumberOfDays);//numberdays - BaseGameEvent.GameEvent.TimeSlot
                if (!ActivitiesSchedule.ContainsKey(index))
                {
                    List<GameSocialEvent> newlist = new List<GameSocialEvent>();
                    newlist.Add(BaseGameEvent.gameEvent);
                    ActivitiesSchedule.Add(index, newlist);
                }
                else
                {
                    List<GameSocialEvent> l;
                    ActivitiesSchedule.TryGetValue(index, out l);
                    l.Add(BaseGameEvent.gameEvent);
                }
            }
        }
           
    }


}
