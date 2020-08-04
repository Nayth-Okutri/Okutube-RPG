using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogManager : MonoBehaviour
{
    public DialogConversation conversation;
    public GameObject speakerLeft;
    public GameObject speakerRight;
    private SpeakerUI speakerUILeft;
    private SpeakerUI speakerUIRight;
    private int activeLineIndex = 0;
    public float timerDisplay;
    public GameObject UIChoice1;
    public GameObject UIChoice2;
    public GameObject UIChoice3;
    private Quizz CurrentQuizz;
    private InputState inputstate;
    [SerializeField]
    private GameObject UpwardPanel;
    private List<GameObject> UIButtons =new List<GameObject>();
    private bool IntroPlayed = false;
    /*[SerializeField]
    private GameEvent OnChoiceChosen;*/
    private enum InputState
    {
        Busy,
        Available,
    }
    // Start is called before the first frame update
    void Start()
    {
        speakerUILeft = speakerLeft.GetComponent<SpeakerUI>();
        speakerUIRight = speakerRight.GetComponent<SpeakerUI>();

        speakerUILeft.Speaker = conversation.LeftPortrait;
        speakerUIRight.Speaker = conversation.RightPortrait;
        timerDisplay = -1.0f;
        HideAllChoices();
        inputstate = InputState.Available;
        UIButtons.Add(UIChoice1);
        UIButtons.Add(UIChoice2);
        UIButtons.Add(UIChoice3);
    }
    void HideAllChoices()
    {
        UIChoice1.SetActive(false);
        UIChoice2.SetActive(false);
        UIChoice3.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if ((timerDisplay >= 0) && (inputstate == InputState.Available))
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                AdvanceConversation();
            }
        }
        if ((Input.GetKeyDown(KeyCode.X)) && (inputstate == InputState.Available))
        {
           
            AdvanceConversation();
        }
    }
    public void DisplayQuestRequirements(QuestDesc quest)
    {
        GameObject ReportLabel = UpwardPanel.transform.Find("ReportText").gameObject;
        Animator PanelMove = UpwardPanel.GetComponent<Animator>();
        PanelMove.SetBool("IsHidden", false);
        ReportLabel.GetComponent<TMPro.TextMeshProUGUI>().SetText(quest.GenerateCompletionDescription());
        conversation = quest.QuestInprogressConversation;
        UpwardPanel.GetComponent<Button>().onClick.AddListener(() => FollowupRequirements());
       
    }
    public void FollowupRequirements()
    {
        if (UpwardPanel != null)
        {
            Animator PanelMove = UpwardPanel.GetComponent<Animator>();
            PanelMove.SetBool("IsHidden", true);
        }
        activeLineIndex = 0;
        AdvanceConversation();
    }


    public void StartPrezConversation()
    {

        if (conversation != null)
        {
            speakerUILeft = speakerLeft.GetComponent<SpeakerUI>();
            speakerUIRight = speakerRight.GetComponent<SpeakerUI>();
            speakerUILeft.Speaker = conversation.LeftPortrait;
            speakerUIRight.Speaker = conversation.RightPortrait;
            activeLineIndex = 0;
            DisplayLine();
            activeLineIndex++;
        }
    }

    public void StartConversation()
    {
        
        if (conversation != null)
        {
            
            speakerUILeft.Speaker = conversation.LeftPortrait;
            speakerUIRight.Speaker = conversation.RightPortrait;
            activeLineIndex = 0;
                DisplayLine();
                activeLineIndex++;
        }
    }

    public void AdvanceConversation()
    {
        if (conversation != null)
        {
            speakerUILeft.Speaker = conversation.LeftPortrait;
            speakerUIRight.Speaker = conversation.RightPortrait;
            if (conversation is ActivityReportDialog)
            {
                ActivityReportDialog DialogReport = conversation as ActivityReportDialog;
                DialogReport.InitializeReport();
                GameObject ReportLabel = UpwardPanel.transform.Find("ReportText").gameObject;
                Animator PanelMove = UpwardPanel.GetComponent<Animator>();
                PanelMove.SetBool("IsHidden", false);
                ReportLabel.GetComponent<TMPro.TextMeshProUGUI>().SetText(DialogReport.lines[0].text);
                UpwardPanel.GetComponent<Button>().onClick.AddListener(() => HideReportDialog());
            }
            else
            {
                if (activeLineIndex < conversation.lines.Length)
                {
                    DisplayLine();
                    activeLineIndex++;
                    timerDisplay = 10f;
                }
                else
                {
                    activeLineIndex = 0;
                    speakerUILeft.Hide();
                    speakerUIRight.Hide();
                    
                    if (conversation.question != null) DisplayQuizz(conversation.question);
                    else HideReportDialog();
                    conversation = null;
                    if (!IntroPlayed)
                    {
                        IntroPlayed = true;
                        TheWorld.Instance.EndIntro();
                    }
                   
                }

            }
            
           
        }
    }
    public void HideReportDialog()
    {
        if (UpwardPanel != null)
        {
            Animator PanelMove = UpwardPanel.GetComponent<Animator>();
            PanelMove.SetBool("IsHidden", true);
            activeLineIndex = 0;
            speakerUILeft.Hide();
            speakerUIRight.Hide();
            conversation = null;
        }

    }
    private void HideDialogUI()
    {
        HideAllChoices();
        speakerUILeft.Hide();
        speakerUIRight.Hide();
    }

    void ChooseChoice(int i)
    {
        if (CurrentQuizz.Choices[i].conversation != null)
        {
            conversation = CurrentQuizz.Choices[i].conversation;
            AdvanceConversation();
        }
        else
        {
            HideDialogUI();
        }
        HideAllChoices();

        inputstate = InputState.Available;
        if (CurrentQuizz.Choices[i].TrigerredQuest != null)
        {
            QuestSystem.Instance.StartQuest(CurrentQuizz.Choices[i].TrigerredQuest);
            NPCController theNPC = GetComponent<NPCController>();
            if (theNPC != null) theNPC.activeQuest = CurrentQuizz.Choices[i].TrigerredQuest;
            //QuestSystem.Instance.
        }
    }

    void DisplayQuizz(Quizz theQuizz)
    {
        CurrentQuizz = theQuizz;
        inputstate = InputState.Busy;
        //speakerUIRight.Show();
        //Ajouter perso perplexe ?
        speakerUILeft.Show();
        int ActiveChoiceIndex=0;
         for (int i =0; i < 3;i++)
        {
            if ((theQuizz.Choices.Length > i) && (ActiveChoiceIndex < theQuizz.Choices.Length))
            {
                while ((QuestSystem.Instance.IsCurrentQuest(theQuizz.Choices[ActiveChoiceIndex].TrigerredQuest)) && (ActiveChoiceIndex<theQuizz.Choices.Length)) ActiveChoiceIndex++;
                int theActiveChoiceIndex = ActiveChoiceIndex;
                UIButtons[i].SetActive(true);
                UIButtons[i].GetComponent<ChoiceUI>().Choice = theQuizz.Choices[ActiveChoiceIndex].text;
                UIButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
                UIButtons[i].GetComponent<Button>().onClick.AddListener(() => { ChooseChoice(theActiveChoiceIndex); });
                if (theQuizz.Choices[ActiveChoiceIndex].TrigerredQuest == null) UIButtons[i].transform.GetChild(1).gameObject.SetActive(false);
              
            }
            ActiveChoiceIndex++;
        }
    }

    void DisplayLine()
    {
        Line line = conversation.lines[activeLineIndex];
        CharacterPortrait character = line.character;
        if (speakerUILeft.SpeakerIs(character)) SetDialog(speakerUILeft, speakerUIRight,line.text);
        else SetDialog(speakerUIRight,speakerUILeft, line.text);
        
    }
    void SetDialog (SpeakerUI activeSpeaker, SpeakerUI inactiveSpeaker, string text)
    {
        activeSpeaker.Dialog = text;
        activeSpeaker.Show();
        inactiveSpeaker.Hide();

    }
}
