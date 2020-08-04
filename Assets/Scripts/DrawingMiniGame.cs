using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingMiniGame : MonoBehaviour
{
    float timer;
    public Transform elementPrototype;
    [SerializeField]
    List<Sprite> ListOfIcons = new List<Sprite>();
    public static List<Sprite> DrawingListOfIcons = new List<Sprite>();
    private Vector3 InitialPosition;
    public static bool canPressButton;
    [SerializeField]
    private GameObject someText;
    [SerializeField]
    private GameObject scoreText;
    int score;
    private float GameTimer;
    [SerializeField]
    private GameEvent OnActivityComplete;
    [SerializeField]
    private GameEvent OnRestoreScene;
    [SerializeField] ConventionItem[] _ConventionItem;
    [SerializeField] GameObject SelectionPanel;
    public Transform SelectionPrototype;
    Vector2 firstItem;
    private bool GameStarted = false;
    private ConventionItem selectedKeyItem;
    [SerializeField] GameObject timerDisplay;
    // Start is called before the first frame update
    void Start()
    {
        InitialPosition = elementPrototype.transform.position;
        elementPrototype.gameObject.SetActive(false);
        foreach (Sprite sprite in ListOfIcons)
        {
            DrawingListOfIcons.Add(sprite);
        }
        GameTimer = 0;
        timer = -1f;
        SelectionPanel.SetActive(true);
        SelectionPrototype.gameObject.SetActive(false);
        firstItem = SelectionPrototype.localPosition;
        InitializeItemSelection();
    }


    void InitializeItemSelection()
    {
        GameObject SelectionLabel = SelectionPanel.transform.Find("SelectionText").gameObject;
        SelectionLabel.GetComponent<TMPro.TextMeshProUGUI>().SetText("Selectionne l'objet que tu souhaites fabriquer");
        var cursor = firstItem;
        int count = 0;
        foreach (var i in _ConventionItem)
        {
            int j = count;
            var e = Instantiate(SelectionPrototype);
            e.transform.SetParent(SelectionPanel.transform);
            e.transform.localPosition = cursor;
            e.transform.GetChild(0).GetComponent<Image>().sprite = i.icon;
            e.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => { SelectKeyItem(j); });
            e.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = i.name;
            e.gameObject.SetActive(true);
            cursor.x += 200;
            count++;
        }
    }

    public void SelectKeyItem(int i)
    {
        selectedKeyItem = _ConventionItem[i];
        HideSelectionScreen();
    }

    public void HideSelectionScreen()
    {
        SelectionPanel.SetActive(false);
        GameStarted = true;
        GameTimer = 10f;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
       if(GameStarted)
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    timer = Random.Range(1f, 3f);
                    var e = Instantiate(elementPrototype);
                    e.transform.position = InitialPosition;
                    e.transform.SetParent(transform);
                    e.gameObject.SetActive(true);
                }
            }
            GameTimer -= Time.deltaTime;
            if (timerDisplay !=null)
            {
                int i= (int)GameTimer;
                timerDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "TIME: " + i.ToString();

            }
            if (GameTimer < 0)
            {
                ActivityManager.Instance.ScoreBonus = score;
                ActivityManager.Instance.ToSendString += " + " + score.ToString();
                Inventory.Instance.AddInventoryKeyItem(selectedKeyItem, 10);
                OnRestoreScene.Raise();
                OnActivityComplete.Raise();
            }
        }
       
    }

    IEnumerator HideText()
    {
        yield return new WaitForSeconds(1f);
        someText.SetActive(false);
    }
    public void PressButton()
    {
        if (canPressButton)
        {
            someText.SetActive(true);
            someText.GetComponent<TMPro.TextMeshProUGUI>().SetText("Good Hit");
            UpdateScore(1);
            StartCoroutine(HideText());
            
        }
        else UpdateScore(-1);
    }
    public void UpdateScore(int i)
    {
        score += i;
        scoreText.GetComponent<TMPro.TextMeshProUGUI>().SetText("Score : " + score.ToString());

    }

}
