using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject DescriptionPanel;
    [SerializeField]
    private GameObject TopRight;
    public GameObject MainMenuPanel;
    public GameObject MainMenu;
    [SerializeField]
    private GameObject UpwardPanel;
    private float timerDisplay;
    public string MyString;
    [SerializeField]
    private GameObject ActivityOKButton;
    [SerializeField]
    private GameObject MoneyCount;
    [SerializeField]
    private GameObject UIInventory;
    [SerializeField]
    private GameObject activeQuestTitle;
    [SerializeField]
    private GameEvent onFinalizeActivity;
    [SerializeField]
    private Sprite RewardPanelImage;
    [SerializeField]
    private Sprite TutorialPanelImage;
    private int MainMenuIndex =0;
    private string[] MainMenuPanelList = new string[3];
    [SerializeField]
    private GameObject InspirationBar;
    [SerializeField]
    private GameObject TeamPanel;
    private int ActiveTopRightPanel = 0;
    GameObject[] TopRightPanels =  new GameObject[3];
    public float TopPanelShuffleDelay = 10f;
    void Start()
    {
        if (DescriptionPanel != null) DescriptionPanel.SetActive(false);
        timerDisplay = 5.0f;
        
        timerDisplay = TopPanelShuffleDelay;
        StartCoroutine(ShuffleTopRightPanel());
    }

    IEnumerator ShuffleTopRightPanel()
    {
        HideTopRightPanel();
        yield return new WaitForSecondsRealtime(1f);
        for (int i=0;i< TopRightPanels.Length;i++)
        {
            if (i == ActiveTopRightPanel) TopRightPanels[i].SetActive(true);
            else TopRightPanels[i].SetActive(false);
        }
        if (ActiveTopRightPanel == 2) RefreshHour();
        ShowTopRightPanel();
        yield return null;
    }
    private void RefreshHour()
    {
        TopRightPanels[2].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().SetText(DateSystem.Instance.GetCurrentTime().ToString());
    }
    public void ShowTopRightPanel()
    {
        Animator anim = TopRight.GetComponent<Animator>();
        anim.SetBool("IsHidden", false);
    }
    public void HideTopRightPanel()
    {
        Animator anim = TopRight.GetComponent<Animator>();
        anim.SetBool("IsHidden", true);
    }
    public void ShowInspirationBar()
    {
        Animator anim = InspirationBar.GetComponent<Animator>();
        anim.SetBool("IsHidden", false);
    }
    public void HideInspirationBar()
    {
        Animator anim = InspirationBar.GetComponent<Animator>();
        anim.SetBool("IsHidden", true);
    }
    public void SetActiveSubMenuIndex(int i)
    {
        MainMenuIndex = i;
        DisplaySubMainMenu();
    }
    public void SetActiveSubMenuIndex1()
    {
        SetActiveSubMenuIndex(0); 
    }
    public void SetActiveSubMenuIndex2()
    {
        SetActiveSubMenuIndex(1);
    }
    public void SetActiveSubMenuIndex3()
    {
        SetActiveSubMenuIndex(2);
    }
    public void DisplayActivityDescription()
    {
        if (UpwardPanel != null)
        {
            UpwardPanel.GetComponent<Image>().sprite = TutorialPanelImage;
            ActivityOKButton.SetActive(true);
            ActivityOKButton.GetComponent<Button>().onClick.AddListener(() => OKActivity());
            GameObject RewardLabel = UpwardPanel.transform.Find("ActivityRewardText").gameObject;
            Animator PanelMove = UpwardPanel.GetComponent<Animator>();
            PanelMove.SetBool("IsHidden", false);
           
            RewardLabel.GetComponent<TMPro.TextMeshProUGUI>().SetText(ActivityManager.Instance.GetCurrentActivityDescription());
            GameObject ToShow = UpwardPanel.transform.Find("TimeClockIcon").gameObject;
            ToShow.SetActive(true);
            ToShow = UpwardPanel.transform.Find("TimeSpentText").gameObject;
            ToShow.SetActive(true);
            ToShow.GetComponent<TMPro.TextMeshProUGUI>().SetText(ActivityManager.Instance.GetCurrentActivityTimeToSpend());

        }

    }

    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                ActiveTopRightPanel = (ActiveTopRightPanel + 1) % (TopRightPanels.Length);
                StartCoroutine(ShuffleTopRightPanel());
                timerDisplay = TopPanelShuffleDelay;
                //HideInspirationBar();

            }
        }
    }
    private void DisplayUpwardMenu(string theContent)
    {
        if (UpwardPanel != null)
        {
            UpwardPanel.GetComponent<Image>().sprite = TutorialPanelImage;
            ActivityOKButton.SetActive(true);
            ActivityOKButton.GetComponent<Button>().onClick.AddListener(() => HideUpwardPanel());
            GameObject RewardLabel = UpwardPanel.transform.Find("ActivityRewardText").gameObject;
            Animator PanelMove = UpwardPanel.GetComponent<Animator>();
            PanelMove.SetBool("IsHidden", false);
            RewardLabel.GetComponent<TMPro.TextMeshProUGUI>().SetText(theContent);
            GameObject Tohide = UpwardPanel.transform.Find("TimeClockIcon").gameObject;
            Tohide.SetActive(false);
            Tohide = UpwardPanel.transform.Find("TimeSpentText").gameObject;
            Tohide.SetActive(false);
        }

    }

    public void DisplayQuestAlreadyCompleted()
    {
        string QuestAlreadyDone = "Cette quête a déjà été complétée";
        DisplayUpwardMenu(QuestAlreadyDone);
    }

    public void DisplayHelpMenu()
    {
        string TutoConstString = "TUTORIAL" + System.Environment.NewLine+ "Bienvenue dans le prototype de RPG, vous incarnez un petit dessinateur prêt à en découdre avec le monde.La JAPEX approche à grand pas, partez à l'aventure et préparer votre stand";
        DisplayUpwardMenu(TutoConstString);
        //Time.timeScale = 0;

    }
    public void UpdateActiveQuest()
    {
        activeQuestTitle.GetComponent<TMPro.TextMeshProUGUI>().SetText( QuestSystem.Instance.getActiveQuestTitle());
    }

    public void RefreshInventoryUI()
    {
        if (Inventory.Instance != null)
            MoneyCount.GetComponent<TMPro.TextMeshProUGUI>().SetText(Inventory.Instance.GetMoneyCount().ToString());

        InventoryUI inventoryUI = UIInventory.GetComponent<InventoryUI>();
        inventoryUI.Refresh();
        Transform KeyItems = MainMenu.transform.GetChild(0).Find("KeyItems");
        if ((KeyItems != null) && (Inventory.Instance != null))
        {
            KeyItems.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().SetText(Inventory.Instance.GetKeyItemTypeCount("Book").ToString() + "p");
            KeyItems.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().SetText(Inventory.Instance.GetKeyItemTypeCount("Goodies").ToString());
        }
    }
    public void OKActivity()
    {
        if (UpwardPanel != null)
        {
            Animator PanelMove = UpwardPanel.GetComponent<Animator>();
            PanelMove.SetBool("IsHidden", true);
        }
        ActivityManager.Instance.ExecuteActivity();
    }



    public void DisplayActivityReward(string theString)
    {
        if (UpwardPanel != null)
        {
            UpwardPanel.GetComponent<Image>().sprite = RewardPanelImage;
            ActivityOKButton.SetActive(false);
            GameObject RewardLabel = UpwardPanel.transform.Find("ActivityRewardText").gameObject;
            GameObject Tohide = UpwardPanel.transform.Find("TimeClockIcon").gameObject;
            Tohide.SetActive(false);
            Tohide = UpwardPanel.transform.Find("TimeSpentText").gameObject;
            Tohide.SetActive(false);
            Animator PanelMove = UpwardPanel.GetComponent<Animator>();
            //timerDisplay += 6f;
            PanelMove.SetBool("IsHidden", false);
            //StatsText = ProgressionSystem.Instance.ProduceSocialStatsText();
            RewardLabel.GetComponent<TMPro.TextMeshProUGUI>().SetText(ActivityManager.Instance.ToSendString);
        }

    }
    private void OnEnable()
    {
        MainMenuPanelList[0] = "ITEMS";
        MainMenuPanelList[1] = "EVENTS";
        MainMenuPanelList[2] = "TEAM";
        TopRightPanels[0] = TopRight.transform.GetChild(0).gameObject;
        TopRightPanels[1] = TopRight.transform.GetChild(1).gameObject;
        TopRightPanels[2] = TopRight.transform.GetChild(2).gameObject;
        for (int i = 0; i < TopRightPanels.Length; i++)
        {
            TopRightPanels[i].SetActive(false);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       if (mode == LoadSceneMode.Additive)
        {
            HideDescription();
            //HideInspirationBar();
        }
      
    }
    public void HideUpwardPanel()
    {
        if (UpwardPanel != null)
        {
            Time.timeScale = 1;
            Animator PanelMove = UpwardPanel.GetComponent<Animator>();
            PanelMove.SetBool("IsHidden", true);
        }

    }
    public void HideActivityReward()
    {
        if (UpwardPanel != null)
        {
            Time.timeScale = 1;
            Animator PanelMove = UpwardPanel.GetComponent<Animator>();
            PanelMove.SetBool("IsHidden", true);
            onFinalizeActivity.Raise();
        }

    }

    public void DisplayCannotAddSideKickInfo()
    {
        // A GENERALISER ?
        string text = "Sidekick ne peut être recruté";
        DisplayDescription(text);


    }
    public void DisplayAddSideKickInfo()
    {
        // A GENERALISER ?
        string text = "Sidekick a été recruté";
        DisplayDescription(text);


    }

    public void DisplayDescription(string Description)
    {
        if (DescriptionPanel != null)
        {
            DescriptionPanel.SetActive(true);
            GameObject DescriptionObject = DescriptionPanel.transform.Find("Description").gameObject;
            DescriptionObject.GetComponent<TMPro.TextMeshProUGUI>().SetText(Description);
         }
    }
    public void HideDescription()
    {
        if (DescriptionPanel != null)
        {
            DescriptionPanel.SetActive(false);
        }

    }

    public void HackChangeScene()
    {
        //BUG ON NE PEUT PAS CLICKER SUR LE BOUTON X
        /*GameObject Player = GameObject.Find("Nay");

        RaycastHit2D hit = Physics2D.Raycast(Player.GetComponent<Rigidbody2D>().position + Vector2.up * 0.2f, new Vector2(1, 0), 1.5f, LayerMask.GetMask("Interactable"));
        if (hit.collider != null)
            SceneTransition.Instance.ForceChangeSceneFromWorldMap(hit.collider.name);*/
    }




    private void DisplaySubMainMenu()
    {
        for (int i=0;i<3;i++)
        {
            GameObject SubMenu = MainMenu.transform.Find(MainMenuPanelList[i]).gameObject;
            if ( i== MainMenuIndex) SubMenu.SetActive(true);
            else SubMenu.SetActive(false);
        }
        if (MainMenuIndex == 1) RefreshInventoryUI();
        if (MainMenuIndex == 2) RefreshTeamMenu();
    }
    private GameObject FindGrandChild()
    {
        // if (transform.childCount >0)
        return null;
    }

    private void BuildTeamMenu()
    {
        GameObject Parent = TeamPanel.transform.Find("SideKicks").gameObject;
        GameObject Element = Parent.transform.Find("Member").gameObject;
        float stepSize = 90f;

        Element.SetActive(false);
        var cursor = Element.transform.localPosition;
        for (var i = 1; i < Parent.transform.childCount; i++)
            Destroy(Parent.transform.GetChild(i).gameObject);
        int count = 0;
       for (int s=0; s< ProgressionSystem.Instance.getSidekicksCount();s++)
        {
            int j = count;
            var e = Instantiate(Element);
            e.gameObject.SetActive(true);
            e.transform.SetParent(Parent.transform);
            e.transform.localPosition = cursor;
            e.transform.localScale = new Vector3(1f, 1f, 1f);
            e.transform.GetChild(0).GetComponent<Image>().sprite = ProgressionSystem.Instance.GetSidekickPortraitAt(s); 
            e.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => { ShowSideKickDetails(j); });
            e.transform.GetChild(0).localPosition = new Vector3(90f, - j * stepSize + 50f, 0f);
            e.transform.GetChild(1).GetComponent<Image>().sprite = ProgressionSystem.Instance.GetSidekickMenuartAt(s);

            e.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = ProgressionSystem.Instance.GetSidekickNametAt(s);
            e.transform.GetChild(2).localPosition = new Vector3(90f, - j * stepSize , 0f);
            e.transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = ProgressionSystem.Instance.GetSidekickSkillAt(s);
            e.transform.GetChild(4).localPosition = new Vector3(120f, - j * stepSize + 80f, 0f);
            e.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => { RemoveSideKick(j); });
            //cursor.y -= stepSize;
            count++;
        }
        ShowSideKick1Details();

    }
    public void RemoveSideKick(int i)
    {
        ProgressionSystem.Instance.RemoveSideKickAt(i);
        BuildTeamMenu();
    }
        public void ShowSideKick1Details()
    {
        GameObject Parent = TeamPanel.transform.Find("SideKicks").gameObject;
        if (Parent.transform.childCount > 1)
        {
            Parent.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            Parent.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
        }
        if (Parent.transform.childCount >2)
        {
            Parent.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
            Parent.transform.GetChild(2).GetChild(3).gameObject.SetActive(false);
        }
       
    }
    public void ShowSideKick2Details()
    {
        GameObject Parent = TeamPanel.transform.Find("SideKicks").gameObject;

        Parent.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        Parent.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
        if (Parent.transform.childCount > 2)
        {
            Parent.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
            Parent.transform.GetChild(2).GetChild(3).gameObject.SetActive(true);
        }
    }
    public void ShowSideKickDetails(int i)
    {
        if (i == 0) ShowSideKick1Details();
        else ShowSideKick2Details();

    }
    private void RefreshTeamMenu()
    {
        BuildTeamMenu();
    }

    public void DisplayHideMainMenu()
    {
        if (MainMenuPanel != null)
        {
            string StatsText;
            GameObject SocialStatsLabel = MainMenuPanel.transform.Find("SocialStatLabel").gameObject;
            GameObject HPText = MainMenuPanel.transform.Find("HP").gameObject;
            Animator PanelMove = MainMenuPanel.GetComponent<Animator>();
            Animator MainMenuMove = MainMenu.GetComponent<Animator>();

            if (!PanelMove.GetBool("IsHidden"))
            {
                PanelMove.SetBool("IsHidden", true);
                MainMenuMove.SetBool("IsHidden", true);
            }
            else
            {
                PanelMove.SetBool("IsHidden", false);
                MainMenuMove.SetBool("IsHidden", false);
                StatsText = ProgressionSystem.Instance.ProduceSocialStatsText();
                SocialStatsLabel.GetComponent<TMPro.TextMeshProUGUI>().SetText(StatsText);
                HPText.GetComponent<TMPro.TextMeshProUGUI>().SetText(ProgressionSystem.Instance.ProduceHPText());
                DisplaySubMainMenu();
            }
        }
    }

    public void UpdateDateDisplay()
    {
        if ((TopRight!=null) && (DateSystem.Instance !=null))
        {
            int RemainingDays = DateSystem.Instance.GetRemainingDays();
            TopRight.SetActive(true);
            ActiveTopRightPanel = 0;
            timerDisplay = TopPanelShuffleDelay;
            TopRightPanels[0].GetComponent<TMPro.TextMeshProUGUI>().SetText(RemainingDays + " DAYS ");

        }

    }

}
