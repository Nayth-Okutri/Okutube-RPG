using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField]
    private Image EnemyHealthmask;
    float EnemyHealthmaskoriginalSize;
    public GameObject PlayerHealthText;
    public GameObject Player;
    public GameObject Enemy;
    public GameObject AttackButton;
    public GameObject RunButton;
    public Canvas PlayerCanvas;
    [SerializeField]
    private GameEvent OnBattleEnd;
    // Start is called before the first frame update
    void Start()
    {
        if (EnemyHealthmask != null) EnemyHealthmaskoriginalSize = EnemyHealthmask.rectTransform.rect.width;
        //Sprite non centré !!!

        AttackButton.GetComponent<RectTransform>().localPosition = GetScreenPositionBasedOffScreenRes(Player, PlayerCanvas, new Vector3(1f,1f,0f));
        RunButton.GetComponent<RectTransform>().localPosition = GetScreenPositionBasedOffScreenRes(Player, PlayerCanvas, new Vector3(0.5f, -0.5f, 0f));
        RunButton.GetComponent<Button>().onClick.AddListener(() => { EscapeAbruptly(); });
        UpdatePlayerHealth(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideUIElements(string tag)
    {
        GameObject[] HidableUIElements = GameObject.FindGameObjectsWithTag("HideUI");
        foreach (GameObject HideUIElement in HidableUIElements)
        {
            HideUIElement.SetActive(false);
        }

    }
    public void UpdateEnemyBar(float value)
    {
        value=Enemy.GetComponent<UnitStats>().health/ Enemy.GetComponent<UnitStats>().maxHealth;
        if (EnemyHealthmask != null) EnemyHealthmask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (EnemyHealthmaskoriginalSize * value));
    }
    public void UpdatePlayerHealth(float value)
    {
        TextMeshProUGUI UIText = PlayerHealthText.GetComponent<TMPro.TextMeshProUGUI>();
        value = Player.GetComponent<UnitStats>().health;
        if (UIText != null) UIText.SetText("" + value);
    }

    public void EscapeAbruptly()
    {
        OnBattleEnd.Raise();
        //SceneManager.LoadScene("MainAtelier");
    }
    private Vector3 GetScreenPositionBasedOffScreenRes(GameObject target, Canvas PlayerCanvas, Vector3 Offset)
    {
        Vector3 offsetPos;
        offsetPos = target.transform.position + Offset;
        offsetPos = new Vector3(4.3f, -1.3f, 0f) + Offset;
       
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
        screenPoint = screenPoint - screenCenter;
        
        return screenPoint;
    }
}
