using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletHell : MonoBehaviour
{
    float GameTimer;
    float timer;
    [SerializeField]
    private GameObject scoreText;
    public GameObject TempUI;
    [SerializeField]
    private GameEvent OnActivityComplete;
    [SerializeField]
    private GameEvent OnRestoreScene;
    public int score;
    [SerializeField] GameObject timerDisplay;
    [SerializeField]
    private GameObject UpwardPanel;
    private bool GameStarted = false;
    // Start is called before the first frame update
    void Start()
    {
       
        UpdateScore(0);
        DisplayIntroInstruction();
    }
    public void DisplayIntroInstruction()
    {
        string IntroductionText = "Bienvenue dans le Bullet hell de l'enfer. Evitez un maximum de tirs pour finir le niveau.";
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
       
        GameTimer = 20f;
        score = 5;
        GameStarted = true; 
    }
    public void ShowSomeUI()
    {
        score--;
        TempUI.SetActive(true);
        TempUI.GetComponent<TMPro.TextMeshProUGUI>().SetText("HIT");

    }
    public void UpdateScore(int i)
    {
        if (score > 0)
        {
            score += i;
            scoreText.GetComponent<TMPro.TextMeshProUGUI>().SetText("Score : " + score.ToString());
        }
       
    }
    void Update()
    {
        if (GameStarted)
        {
            timer -= Time.deltaTime;


            if (timer < 0)
            {
                timer = Random.Range(1.0f, 4.0f);
                TempUI.SetActive(false);
            }
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
                OnRestoreScene.Raise();
                OnActivityComplete.Raise();
            }

        }
      
    }
}
