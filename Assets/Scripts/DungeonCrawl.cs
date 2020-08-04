using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DungeonCrawl : MonoBehaviour
{
    private float GameTimer;
    [SerializeField] GameObject timerDisplay;
    private bool GameStarted=false;
    [SerializeField]
    private GameObject scoreText;
    int score;
    [SerializeField]
    private GameEvent OnRestoreScene;
    [SerializeField]
    private GameObject UpwardPanel;

    
    void Start()
    {


        DisplayIntroInstruction();
    }

    public void DisplayIntroInstruction()
    {
        string IntroductionText="Bienvenue dans le dungeon Crawler de l'enfer. Trouvez le gros méchant monstre pour finir le niveau.";
        GameObject ReportLabel = UpwardPanel.transform.Find("ReportText").gameObject;
        Animator PanelMove = UpwardPanel.GetComponent<Animator>();
        PanelMove.SetBool("IsHidden", false);
        ReportLabel.GetComponent<TMPro.TextMeshProUGUI>().SetText(IntroductionText);
       
        UpwardPanel.GetComponent<Button>().onClick.AddListener(() => FollowupInstruction());
        UpwardPanel.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => FollowupInstruction());
        Time.timeScale = 0;
    }
    public void FollowupInstruction()
    {
        if (UpwardPanel != null)
        {
            Animator PanelMove = UpwardPanel.GetComponent<Animator>();
            PanelMove.SetBool("IsHidden", true);
        }
        Time.timeScale = 1;
        GameTimer = 180f;
        GameStarted = true;
    }

    public void DisplayScoreScreen()
    {
        if (GameStarted)
        {
            GameStarted = false;
            int temps = 180 - (int)GameTimer;
            string ResultText = "Bravo vous avez fini le niveau en " + temps.ToString() + "s";
            GameObject ReportLabel = UpwardPanel.transform.Find("ReportText").gameObject;
            Animator PanelMove = UpwardPanel.GetComponent<Animator>();
            PanelMove.SetBool("IsHidden", false);
            ReportLabel.GetComponent<TMPro.TextMeshProUGUI>().SetText(ResultText);
            UpwardPanel.GetComponent<Button>().onClick.RemoveAllListeners();
            UpwardPanel.GetComponent<Button>().onClick.AddListener(() => FollowupScoreScreen());
            UpwardPanel.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => FollowupScoreScreen());
            Time.timeScale = 0;
        }
       
    }
    public void FollowupScoreScreen()
    {
        Time.timeScale = 1;
        if (UpwardPanel != null)
        {
            Animator PanelMove = UpwardPanel.GetComponent<Animator>();
            PanelMove.SetBool("IsHidden", true);
        }
        DateSystem.Instance.IncreaseTime(10);
        OnRestoreScene.Raise();
    }
    void Update()
    {
        if (GameStarted)
        {
            
            GameTimer -= Time.deltaTime;
            if (timerDisplay != null)
            {
                int i = (int)GameTimer;
                timerDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "TIME: " + i.ToString();

            }
            if (GameTimer < 0)
            {
                ActivityManager.Instance.ScoreBonus = score;
                ActivityManager.Instance.ToSendString += " + " + score.ToString();
                DisplayScoreScreen();
                //OnActivityComplete.Raise();
            }
        }

    }
}
