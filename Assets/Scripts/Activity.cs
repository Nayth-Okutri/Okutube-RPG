using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Activity : MonoBehaviour
{
    public ActivityDesc baseActivity;
    private string Animation;

    [SerializeField]
    private bool permanent;
    public bool Permanent { get { return permanent; }  }

    private ActivityState activityState;
    private enum ActivityState
    {
        Ongoing,
        Free,
    }
    [SerializeField]
    private Cutscene cutscene;

    // Start is called before the first frame update

    void Start()
    {
       
        activityState = ActivityState.Free;
    }
    void Awake()
    {
        
    }

    public void RewardActivity()
    {
        // une seule activité en cours a la fois contrainte ou avoir plusieurs event selon type d activités en parallele
        if (activityState==ActivityState.Ongoing)
        {
            // Ajouter du rdm

            // ProgressionSystem.Instance.InscreaseSocialStat(baseActivity.statIncrease.name, baseActivity.statIncreaseValue + ActivityManager.Instance.ScoreBonus);
            baseActivity.GiveRewards();
            //NE PAS OUBLIER DE REINIT SCORE BONUS
            if ((baseActivity.Reward != null) && (baseActivity.Reward.statRewards.Length >0))  ProgressionSystem.Instance.InscreaseSocialStat(baseActivity.Reward.statRewards[Random.Range(0, baseActivity.Reward.statRewards.Length-1)].stat.name, ActivityManager.Instance.ScoreBonus);
            
            DateSystem.Instance.IncreaseTime(baseActivity.TimeSpent);
            activityState = ActivityState.Free;
            ActivityManager.Instance.NotifyActivityCompleted(baseActivity);
            ActivityManager.Instance.ScoreBonus = 0;
            TheWorld.Instance.ShowMainCharacter();
            TheWorld.Instance.ShowAllNPC();
        }
    }


    IEnumerator RunCutscene()
    {
        if (cutscene != null)
        {
            cutscene.StartCutscene(SceneManager.GetActiveScene().buildIndex);
            yield return new WaitForSeconds(cutscene.AnimationDuration);
            cutscene.EndCutscene();
        }
        ActivityManager.Instance.ActivityCompleted();

    }

    private void RunMinigame()
    {
        foreach (MiniGame someGame in baseActivity.Challenges)
        {
            if ((someGame.MinigameScene!=null) && (someGame.MinigameScene != ""))
            {
                //VRAI MINIGAME
                //if (gameObject.transform.parent == null) DontDestroyOnLoad(gameObject);
                ActivityManager.Instance.ToSendString = "Félicitation, vos récompenses sont les suivantes " + System.Environment.NewLine + baseActivity.GetRewardNames();
                SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
                if (sprite!=null) sprite.sortingOrder = -100;
                SceneTransition.Instance.LoadSceneForATime(someGame.MinigameScene, new Vector3(0f,0f,0f));
                
            }
            else {
                TheWorld.Instance.HideMainCharacter();
                TheWorld.Instance.HideAllNPC();
                ActivityManager.Instance.ToSendString = "Félicitation, vos récompenses sont les suivantes " + System.Environment.NewLine + baseActivity.GetRewardNames();
                StartCoroutine(RunCutscene());
                
            }
        }
    }

    public void DoActivity()
    {
        if (activityState != ActivityState.Ongoing)
        {
            TheWorld.Instance.HideAllNPC();
            RunMinigame();
            activityState = ActivityState.Ongoing;
        }
    }

    public string ProduceActivityRequirements()
    {
        string _result;
        _result = baseActivity.description;
        _result += System.Environment.NewLine + "elle peut vous rapporter de " + baseActivity.GetStatRewardNames() ;
        return _result;
    }
}
