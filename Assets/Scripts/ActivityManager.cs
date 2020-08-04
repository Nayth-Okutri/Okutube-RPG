using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivityManager : MonoBehaviour
{
    public static ActivityManager Instance;
    float timer;
    public Vector3 AtelierPosition;

    [SerializeField]
    private GameEvent OnActivityComplete;
    [SerializeField]
    private GameEvent OnActivityQuery;
   
    public string ToSendString;
    private List<Activity> AvailableActivities = new List<Activity>();
    private Activity currentActivity;
    public int ScoreBonus;
    // Start is called before the first frame update

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
        timer = -1.0f;

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void OnSceneUnloaded(Scene scene)
    {
        UnloadAllSceneActivities();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadAllSceneActivities();
        InitializeActivities();
    }
    public void InitializeActivities()
    {
        if (TheWorld.Instance != null)
        { 
        foreach (Activity theActivity in AvailableActivities)
            {
                if (!theActivity.Permanent)
                    if (!TheWorld.Instance.IsGameSocialEventAvailableToday(theActivity.baseActivity))
                        theActivity.gameObject.SetActive(false);
            }
         }
    }
    public string GetCurrentActivityDescription()
    {
        string Description="";
        if (currentActivity != null) Description=currentActivity.ProduceActivityRequirements();
        return Description; 
    }
    public string GetCurrentActivityTimeToSpend()
    {
        string Description = "";
        if (currentActivity != null) Description = "Temps dépensé : " + currentActivity.baseActivity.TimeSpent.ToString();
        return Description;
    }
    public void LoadAllSceneActivities()
    {
        GameObject[] activites = GameObject.FindGameObjectsWithTag("Activity");
        foreach (GameObject theActivity in activites)
        {
            Activity currentActivity = theActivity.GetComponent<Activity>();
            AvailableActivities.Add(currentActivity);
        }
    }

    public void NotifyActivityCompleted(ActivityDesc activity)
    {
        if (currentActivity.baseActivity == activity)
        {
            currentActivity = null;
            ScoreBonus = 0;
        }

    }
    public void StartActivity(Activity activity)
    {
        currentActivity = activity;
        OnActivityQuery.Raise();
    }

    public void ExecuteActivity()
    {
        if (currentActivity != null)
            currentActivity.DoActivity();
    }

    public void OKActivity()
    {
        if (currentActivity!=null)
        currentActivity.DoActivity();
    }

    public void UnloadAllSceneActivities()
    {
        AvailableActivities.Clear();
    }

    public void FinalizeActivity()
    {
        if (currentActivity != null)
        {
            currentActivity.RewardActivity();

        }

    }
    private void Awake()
    {
        LoadAllSceneActivities();
    }

    public void OnLoadScreen()
    {
     
    }


    // Update is called once per frame
    void Update()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                // 
                
            }
        }
    }

    public void ActivityCompleted()
    {
        if (OnActivityComplete != null)
        {
            OnActivityComplete.sentString = ToSendString;
            OnActivityComplete.Raise();
        }

    }

}
