using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class QuestSystem : MonoBehaviour
{
    private List<QuestDesc> activeQuests = new List<QuestDesc>();
    public QuestDesc theCurrentQuest;
    /*public string title;
    public string desc;
    /*public ItemRequirement[] requiredItems;
    public InventoryItem[] rewardItems;*/
    public static QuestSystem Instance;
    public GameEvent OnQuestStarted;
    public GameEvent OnQuestAlreadyDone;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
       
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (activeQuests.Count > 0)  OnQuestStarted.Raise();
    }


    public bool IsCurrentQuest(QuestDesc quest)
    {
        return activeQuests.Contains(quest);
    }
    private void RemoveActiveQuest(QuestDesc quest)
    {
         activeQuests.Remove(quest);
    }
    public bool CheckRequirements(QuestDesc activeQuest)
    {
        bool AllOK = true;
        if ((IsCurrentQuest(activeQuest)) && (activeQuest != null))
        {
            for (int i = 0; i < activeQuest.completionRequirements.itemRequirement.Length; i++)
            {
                if (Inventory.Instance.GetInventoryCount(activeQuest.completionRequirements.itemRequirement[i].item) < activeQuest.completionRequirements.itemRequirement[i].itemCount)
                {
                    AllOK = false;
                    break;
                }
            }
            if (AllOK)
            {
                for (int i = 0; i < activeQuest.completionRequirements.socialStatRequirement.Length; i++)
                {
                    if (ProgressionSystem.Instance.GetSocialStatValue(activeQuest.completionRequirements.socialStatRequirement[i].stat.name) < activeQuest.completionRequirements.socialStatRequirement[i].statAmount)
                    {
                        AllOK = false;
                        break;
                    }
                }
            }
           
        }
        return AllOK;
       
    }
    public void StartQuest(QuestDesc q)
    {
        if ((!q.Completed) && (!activeQuests.Contains(q)))
        {
            activeQuests.Add(q);
            OnQuestStarted.sentInt = q.QuestId;
            OnQuestStarted.Raise();
        }
        else
        {
            //WARN PLAYER ALREADY DONE
            OnQuestAlreadyDone.Raise();
        }
    }


    public void GiveRewards(QuestDesc activeQuest)
    {
        if ((IsCurrentQuest(activeQuest)) && (activeQuest != null))
        {
            if (activeQuest.Reward.sideKick != null)
            {
                ProgressionSystem.Instance.AddSideKick(activeQuest.Reward.sideKick);
            }
            activeQuest.GiveRewards();
        }
    }

    public void OnFinishQuest()
    {
        GiveRewards(theCurrentQuest);
        RemoveActiveQuest(theCurrentQuest);
        theCurrentQuest = null;
        OnQuestStarted.Raise();
    }
    public string getActiveQuestTitle()
    {
        string result="";
        //if (thCurrentQuest != null) result = thCurrentQuest.fullname;
        foreach (var q in activeQuests) result += System.Environment.NewLine + "- " + q.fullname;
       
        return result;
    }
    public int getActiveQuestId()
    {
        int result;
        if (theCurrentQuest != null) result = theCurrentQuest.QuestId;
        else result = 0;
        return result;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
