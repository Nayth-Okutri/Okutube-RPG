using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;
    float timer;
    string SceneToLoad;
    Scene SceneToRestore;
    GameObject MainCharacter;
    public GameObject SceneTransitionObject;
    private Dictionary<int, Vector3> SpawningPositions;
    const int WorldmapIndex = 4;
    [SerializeField]
    private GameObject DateChangeObject;
    public AudioClip ChangeDayFXClip;
    public AudioClip ChangeSceneClip;
    private Dictionary<GameObject, bool> previousSceneObjects;
    private IEnumerator Loadingcoroutine;
    private Vector3 WorldMapStartingPosition= new Vector3(-0.71f, -1.30f, 0f);
    private Vector3 AteliertartingPosition = new Vector3(-3f, 0f, 0f);
    private Dictionary<string, string> ScenesDisplayNames = new Dictionary<string, string>();
    [SerializeField]
    private Cutscene[] NewDayCutscenes=new Cutscene[2];
    private bool CanGotoDungeon =true;


    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            
            SpawningPositions=new Dictionary<int, Vector3>();
            SpawningPositions.Add(WorldmapIndex, WorldMapStartingPosition);
        }
        else
        {
            //Destroy(IntroAnimation);
        }
        timer = -1.0f;
        
        
    }
    private void OnDestroy()
    {
       
    }


    public void StartNewDay()
    {
        //Reset worldmap position
      
        if (SpawningPositions.ContainsKey(WorldmapIndex)) SpawningPositions[WorldmapIndex] = WorldMapStartingPosition;
        else SpawningPositions.Add(WorldmapIndex, WorldMapStartingPosition);
        SpawningPositions[1] = AteliertartingPosition;
        StartCoroutine(ChangeDayScene());
        
    }

    public void StartDungeonCrawl()
    {
        if (CanGotoDungeon)
        {
            Animator transition;
            transition = DateChangeObject.GetComponent<Animator>();
            transition.SetBool("IsHidden", true);
            if (SceneTransitionObject != null)
            {
                transition = SceneTransitionObject.GetComponent<Animator>();
                if (transition != null) transition.SetTrigger("Restart");
            }
            DateChangeObject.SetActive(false);
            CanGotoDungeon = false;
            SceneManager.LoadScene("GameDungeon");
        }
    }


    public void HideDateChanger()
    {
        Animator transition;
        transition = DateChangeObject.GetComponent<Animator>();
        transition.SetBool("IsHidden", true);
        if (SceneTransitionObject != null)
        {
            transition = SceneTransitionObject.GetComponent<Animator>();
            if (transition != null) transition.SetTrigger("Restart");
        }
        DateChangeObject.SetActive(false);
        StartCoroutine(RunIntroAnimation());
    }
    IEnumerator RunIntroAnimation()
    {

        TheWorld.Instance.HideMainCharacter();
        TheWorld.Instance.HideAllNPC();
        Cutscene NewDayCutscene = NewDayCutscenes[Random.Range(0, NewDayCutscenes.Length)];
        //NewDayCutscene = NewDayCutscenes[1];
        NewDayCutscene.StartCutscene(1);
        yield return new WaitForSeconds(NewDayCutscene.AnimationDuration);
        NewDayCutscene.EndCutscene();
       
        TheWorld.Instance.ShowAllNPC();
        TheWorld.Instance.ShowMainCharacter();
        yield return null;
    }
    IEnumerator ChangeDayScene()
    {
        //Animation NEW DAY
        Animator transition;
        if (Loadingcoroutine != null) StopCoroutine(Loadingcoroutine);
        if (DateChangeObject!=null)
        {
            DateChangeObject.SetActive(true);
            GameObject NewDateText = DateChangeObject.transform.Find("NewDateText").gameObject;
            GameObject DungeonButton = DateChangeObject.transform.Find("DungeonButton").gameObject;
            if (!CanGotoDungeon) DungeonButton.SetActive(false);
            else DungeonButton.SetActive(true);
            NewDateText.SetActive(false);
            transition = DateChangeObject.GetComponent<Animator>();
            if (transition != null)
            {
                //transition.SetTrigger("Start");
                transition.SetBool("IsHidden", false);
            }
            AudioSource audio = DateChangeObject.GetComponent<AudioSource>();
            audio.PlayOneShot(ChangeDayFXClip);
            yield return new WaitForSeconds(1f);
            NewDateText.SetActive(true);
            NewDateText.GetComponent<TMPro.TextMeshProUGUI>().SetText("New DAY " + DateSystem.Instance.getCurrentDay().ToString());
           
        }
        if (Loadingcoroutine != null) StopCoroutine(Loadingcoroutine);
        CanGotoDungeon = true;
        SceneManager.LoadScene("MainAtelier");
        

        yield return null;
    }

    public void RestorePreviousScene()
    {
        if (SceneToLoad != null)
        {
            EnableOldScene();
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneToRestore.name));
            ActivityManager.Instance.OnLoadScreen();
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(SceneToLoad));
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                //degagé  RestorePreviousScene

            }
        }
    }
    public void ForceChangeSceneFromWorldMap(string Source)
    {
        GameObject Nay = GameObject.Find("Nay");
        SceneTransition.Instance.ChangeScene(Source, Nay.transform.position);

    }
    public void ForceChangeSceneToWorldMap()
    {
        StartCoroutine(LoadLevel("Worldmap"));
    }
    public string GetDestinationName(string SourcePoint)
    {
        return ScenesDisplayNames[GetDestination(SourcePoint)];
    }

        public string GetDestination(string SourcePoint)
    {
        string SceneToLoad = "";
        if (SourcePoint == "WorldmapSortie") SceneToLoad = "Worldmap";
        else if (SourcePoint == "Atelier") SceneToLoad = "MainAtelier";
        else if (SourcePoint == "Desktop") SceneToLoad = "DeskAtelier";
        else if (SourcePoint == "BookShop") SceneToLoad = "BookShop";
        else if (SourcePoint == "Beach") SceneToLoad = "Beach";
        else if (SourcePoint == "Cafe") SceneToLoad = "Cafe";
        else if (SourcePoint == "Tavern") SceneToLoad = "Tavern";
        else if (SourcePoint == "WeaponShop") SceneToLoad = "WeaponShop";
        else if (SourcePoint == "Expo") SceneToLoad = "JapexHall";

      
        return SceneToLoad;

    }
    private void SaveOriginPosition( Vector3 position)
    {
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;
    
        if (SpawningPositions.ContainsKey(SceneIndex)) SpawningPositions[SceneIndex] = position;
        else SpawningPositions.Add(SceneIndex, position);
    }

    public Vector3 RestoreSpawningPosition()
    {
        Vector3 position = WorldMapStartingPosition;
        GameObject OriginalSpawningPoint = GameObject.Find("WorldmapSortie");
        if (OriginalSpawningPoint != null) position = OriginalSpawningPoint.transform.position;
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (SpawningPositions.ContainsKey(SceneIndex))  return SpawningPositions[SceneIndex];
        else return position;
    }
    IEnumerator LoadLevel(string scene)
    {
        Animator transition;
        if (SceneTransitionObject ==null) SceneTransitionObject= GameObject.Find("CrossFade");

        if (SceneTransitionObject != null)
        {
            transition = SceneTransitionObject.GetComponent<Animator>();
            //if (transition != null) transition.SetTrigger("Start");
            if (transition != null) transition.SetBool("ScreenIsHidden", true);
            AudioSource audio = SceneTransitionObject.GetComponent<AudioSource>();
            audio.PlayOneShot(ChangeSceneClip);
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(scene);
            //if (transition != null) transition.SetTrigger("Restart");
        }
        else SceneManager.LoadScene(scene);
        
    }

    public void ChangeScene(string SourcePoint, Vector3 position)
    {
        SaveOriginPosition(position);
        string SceneToLoad="";
        SceneToLoad = GetDestination(SourcePoint);
        DateSystem.Instance.IncreaseTime(1);
        Loadingcoroutine = LoadLevel(SceneToLoad);
        StartCoroutine(Loadingcoroutine);
    }

    public void LoadSceneForATime(string NewScene,Vector3 position)
    {
        //SaveOriginPosition(position);
        if (NewScene != "")
        {
            Animator transition;
            SceneToRestore = SceneManager.GetActiveScene();
            SceneToLoad = NewScene;
            if (SceneTransitionObject != null)
            {
                transition = SceneTransitionObject.GetComponent<Animator>();
                //if (transition != null) transition.SetTrigger("Start");
                if (transition != null) transition.SetBool("ScreenIsHidden",true);
                //if (transition != null) transition.SetTrigger("Restart");
            }
            DisableOldScene();
            //SceneManager.LoadSceneAsync(NewScene, LoadSceneMode.Additive);
            StartCoroutine(LoadSceneAsynch(NewScene));
        }
    }

    IEnumerator LoadSceneAsynch(string NewScene)
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(NewScene, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
                
            }
            yield return null;
        }
    }





    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Animator transition;
        if (SceneTransitionObject != null)
        {
            transition = SceneTransitionObject.GetComponent<Animator>();
            if (transition != null) transition.SetBool("ScreenIsHidden", false);

        }
        if (mode != LoadSceneMode.Additive)
        {
            previousSceneObjects = new Dictionary<GameObject, bool>();
        }
    }
    private void DisableOldScene()
    {
        if (SceneToRestore.IsValid())
        {
            previousSceneObjects = new Dictionary<GameObject, bool>();
            GameObject[] oldSceneObjects = SceneToRestore.GetRootGameObjects();
            for (int i = 0; i < oldSceneObjects.Length; i++)
            {
                previousSceneObjects.Add(oldSceneObjects[i], oldSceneObjects[i].activeInHierarchy);
                if ((oldSceneObjects[i].name != "_Preload") && (oldSceneObjects[i].name != "UIOverlay")) oldSceneObjects[i].SetActive(false);
            }
            TheWorld.Instance.HideMainCharacter();
          
        }
    }
    private void EnableOldScene()
    {
        if (SceneToRestore.IsValid())
        {
            foreach (KeyValuePair<GameObject, bool> entry in previousSceneObjects)
            {
                if (entry.Value) entry.Key.SetActive(true);
               
            }
            TheWorld.Instance.ShowMainCharacter();
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        ScenesDisplayNames.Add("MainAtelier", "Atelier Dessin");
        ScenesDisplayNames.Add("JapexHall", "Japexpo Hall");
        ScenesDisplayNames.Add("Worldmap", "Worldmap");
        ScenesDisplayNames.Add("BookShop", "Boutique");
        ScenesDisplayNames.Add("WeaponShop", "Boutique");
        ScenesDisplayNames.Add("Tavern", "Taverne");
        ScenesDisplayNames.Add("Cafe", "Cafe");
        ScenesDisplayNames.Add("Beach", "Plage");
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;

    }
    void OnSceneUnloaded(Scene scene)
    {

    }

}
