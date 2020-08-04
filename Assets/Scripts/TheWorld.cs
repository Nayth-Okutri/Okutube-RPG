using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheWorld : MonoBehaviour
{
    public static TheWorld Instance;
    [SerializeField]
    private GameObject PlayerController;
    [SerializeField]
    private GameObject CameraConfiners;
    [SerializeField]
    private GameObject LevelLoaders;
    [SerializeField]
    public GameCalendar gameCalendar;
    private GameObject[] NPCLoaded;
    [SerializeField]
    private GameObject MainCharacter;
    private List<int> MobSpawnDays = new List<int>();
    public int CurrentDayMobCount;
    private List<Vector3> ListOfSpawningPostion;

    // LOL en attendant d avoir des timelines ?
    private bool IntroPlayed;
    [SerializeField]
    private GameObject IntroFab;
    private GameObject TempObject;
    public DialogConversation IntroductionDialog;
    //private Dictionary<int, string> ScenesDisplayNames=new Dictionary<int, string>();
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            if (PlayerController!=null) DontDestroyOnLoad(PlayerController);
            if (LevelLoaders != null) DontDestroyOnLoad(LevelLoaders);
            if (CameraConfiners != null) DontDestroyOnLoad(CameraConfiners);
            if ((!IntroPlayed) && (IntroFab != null) && (SceneManager.GetActiveScene().buildIndex == 1)) PlayIntro();
        }
        else
        {
            Destroy(gameObject);
            Destroy(PlayerController);
        }

        //MainCharacter  = GetMainCharacterObject();

       
    }
    private void PlayIntro()
    {
        HideMainCharacter();
        HideAllNPC();
        TempObject = Instantiate(IntroFab);
        TempObject.transform.position = new Vector3(-3.9f, 0.77f, 0f);
        DialogManager theDialogManager = GameObject.Find("DialogSystem").GetComponent<DialogManager>();
        theDialogManager.conversation = IntroductionDialog;
        theDialogManager.timerDisplay = 5f;
        theDialogManager.StartPrezConversation();
        IntroPlayed = true;
        IntroFab = null;
    }

public void EndIntro()
    {
        ShowMainCharacter();
        ShowAllNPC();
        if (TempObject!=null) TempObject.SetActive(false);
    }
    public void HideAllNPC()
    {
        foreach (GameObject NPC in NPCLoaded)
        {
            NPC.SetActive(false);
        }
    }

    public void ShowAllNPC()
    {
        foreach (GameObject NPC in NPCLoaded)
        {
            NPC.SetActive(true);
        }
    }

    public bool IsGameSocialEventAvailableToday(GameSocialEvent g)
    {
        bool result=false;
        List<GameSocialEvent> l;
        int currentDay = 0;
        if (DateSystem.Instance != null) currentDay = DateSystem.Instance.getCurrentDay();
        if (gameCalendar.ActivitiesSchedule.TryGetValue(currentDay, out l))
        {
            result = l.Contains(g);
        }
        return result;
    }
    public string GenerateCalendarReport(int i)
    {
        string result = "";
        List<GameSocialEvent> l;
        if (gameCalendar.ActivitiesSchedule.TryGetValue(i, out l))
        {
            for (int j = 0; j < l.Count; j++)
            {
                if (result == "") result += "Jour " + i.ToString() + " " + l[j].fullname + System.Environment.NewLine;
                else result += l[j].fullname + System.Environment.NewLine;
            }
        }

        return result;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        ListOfSpawningPostion = new List<Vector3>();

        ListOfSpawningPostion.Add(new Vector3(Random.Range(1, 2), Random.Range(-3, -4), 0));
        ListOfSpawningPostion.Add(new Vector3(Random.Range(-5, -4), Random.Range(-5, -4), 0));
        ListOfSpawningPostion.Add(new Vector3(Random.Range(4, 5), Random.Range(0, 1), 0));
        ListOfSpawningPostion.Add(new Vector3(Random.Range(-2, -1), Random.Range(-2, -1), 0));
        ListOfSpawningPostion.Add(new Vector3(Random.Range(0, 0), Random.Range(0, 0), 0));
        //IntroPlayed = false;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private GameObject GetMainCharacterObject()
    {
        return GameObject.Find("Nay");
    }

    private void SetDefaultPlayerParameters()
    {
        if (PlayerController != null)
        {
            PlayerController.transform.position = new Vector3(0f, 0f, 0f);
            if (MainCharacter != null)
            {
                MainCharacter.transform.localScale = new Vector3(0.56f, 0.56f, 0.56f);
                MainCharacter.GetComponent<NayCharController>().speed = 3.0f;
                //Order of loading
                if (SceneTransition.Instance !=null) MainCharacter.transform.position = SceneTransition.Instance.RestoreSpawningPosition();
                MainCharacter.GetComponent<NayCharController>().SetInputAvailable();
            }
        }
    }

    private void SetWorldmapPlayerParameters()
    {
        // GameObject player = GetMainCharacterObject();
        if (MainCharacter != null)
        {
            MainCharacter.SetActive(true);
            MainCharacter.GetComponent<NayCharController>().speed = 1.0f;
            MainCharacter.transform.localScale = new Vector3(0.34f, 0.34f, 0.34f);
        }
    }
    public void ShowMainCharacter()
    {
        PlayerController.SetActive(true);

    }
    public void HideMainCharacter()
    {
        PlayerController.SetActive(false);

    }
    private void SpawnNMy(Vector3 spawnPosition)
    {
        
        GameObject enemy1 = ObjectPooler.Instance.GetPooledObject("Mob");
        Quaternion spawnRotation = Quaternion.Euler(0, 0, 0);
        if (enemy1 != null)
        {
            enemy1.transform.position = spawnPosition;
            enemy1.transform.rotation = spawnRotation;
            enemy1.SetActive(true);
        }

    }
    private void SpawnEnemies()
    {

        //Ennemies non persistant donc les pooled object meurent entre les scnes... a reconsidérer

        if (DateSystem.Instance != null) 
        {
            if (!MobSpawnDays.Contains(DateSystem.Instance.getCurrentDay())) CurrentDayMobCount = Random.Range(1, 5);
        }
        if ((ObjectPooler.Instance!=null)&&(ObjectPooler.Instance.pooledObjects == null)) ObjectPooler.Instance.Init();
        

        //if ((DateSystem.Instance != null) && (!MobSpawnDays.Contains(DateSystem.Instance.getCurrentDay()))) DoSpawn = true;
        for (int i=0;i < CurrentDayMobCount;i++ )
        {
            SpawnNMy(ListOfSpawningPostion[Random.Range(0, ListOfSpawningPostion.Count-1)]);
        }
        if (DateSystem.Instance == null) MobSpawnDays.Add(0);
        //Facto
        else if (!MobSpawnDays.Contains(DateSystem.Instance.getCurrentDay())) MobSpawnDays.Add(DateSystem.Instance.getCurrentDay());

    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if ((Instance == this) || (Instance==null))
        {
            if (MainCharacter == null) MainCharacter = GetMainCharacterObject();
            if ((scene.name == "BattleScene") ||  (scene.name == "MiniGame01") || (scene.name == "MiniGame02"))
            {
                if (PlayerController != null) PlayerController.SetActive(false);

            }
            else
            {
                SetDefaultPlayerParameters();
                if (PlayerController != null) PlayerController.SetActive(true);
                if (scene.name == "Worldmap")
                {
                    SpawnEnemies();
                    SetWorldmapPlayerParameters();
                }

                if (scene.name == "MainAtelier")
                {
                    //if ((ProgressionSystem.Instance != null) && (ProgressionSystem.Instance.sideKicks.Count > 0))
                    {
                        //GameObject npc = ProgressionSystem.Instance.sideKicks[0].CharacterPrefab;
                        //var e = Instantiate(npc);
                        //e.transform.SetParent(transform);
                        //e.transform.localPosition = new Vector3(0f,0f,0f);

                    }
                }
                if (scene.name == "GameDungeon")
                {
                    MainCharacter.GetComponent<NayCharController>().speed = 4.0f;
                    MainCharacter.transform.localScale = new Vector3(0.34f, 0.34f, 0.34f);
                }


            }
            if (mode != LoadSceneMode.Additive)
            NPCLoaded = GameObject.FindGameObjectsWithTag("NPC");
            if (TempObject != null) TempObject.SetActive(false);
        }
             

    }
 
}
