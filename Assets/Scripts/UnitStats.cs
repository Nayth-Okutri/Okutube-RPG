using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UnitStats : MonoBehaviour, IComparable
{
    public int health;
    public float mana;
    public float attack;
    public float magic;
    public float defense;
    public float speed;
    public int nextActTurn;
    private Animator animator;
    [SerializeField]
    private GameObject damageTextPrefab;
    private Vector2 damageTextPosition;
    public GameObject UIHealth;
    private bool dead = false;
    public bool unTargetable;
    public float maxHealth;
    public GameObject DamageText;
    float timerDisplay;

    void Start()
    {
        animator = GetComponent<Animator>();
        maxHealth = health;
        timerDisplay = -1.0f;
        if (gameObject.name == "NayBattle") InitializeStatsFromPlayerStats();


    }
    public void UpdateCharacter()
    {
        if (gameObject.name == "NayBattle") ProgressionSystem.Instance.SetHealthStat(health); 

    }
    void InitializeStatsFromPlayerStats()
    {
        health = ProgressionSystem.Instance.GetHealthStat();
        attack= ProgressionSystem.Instance.GetStrengthStat();
        defense = ProgressionSystem.Instance.GetDefenceStat();
        speed = ProgressionSystem.Instance.GetSpeedStat();
    }
  
    // Update is called once per frame
    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                DamageText.SetActive(false);
            }
        }
    }

    public void calculateNextActTurn(int currentTurn)
    {
        this.nextActTurn = currentTurn + (int)Math.Ceiling(100.0f / this.speed);
        Debug.Log("NextActTurn de " + this.gameObject.name + ":" + this.nextActTurn);
    }


    public int CompareTo(object otherStats)
    {
        return nextActTurn.CompareTo(((UnitStats)otherStats).nextActTurn);
    }

    public int Compare(object a, object b)
    {
 
        return ((UnitStats)a).nextActTurn.CompareTo(((UnitStats)b).nextActTurn);
    }
    public bool IsDead()
    {
        return this.dead;
    }
    public void VictoryPose()
    {
        animator.SetTrigger("Victory");

    }

    IEnumerator DelayHitAnimation()
    {

        yield return new WaitForSeconds(0.5f);
        if (animator != null) animator.SetTrigger("Hit");
    }

    public void receiveDamage(float damage)
    {
        this.health -= (int)damage;
       
        StartCoroutine(DelayHitAnimation());
        GameObject HUDCanvas = GameObject.Find("HUDCanvas");
        if (HUDCanvas != null)
        {
        }
        if (DamageText != null)
        {
            DamageText.SetActive(true);
            timerDisplay = 2f;
            DamageText.GetComponent<TMPro.TextMeshProUGUI>().SetText("" + damage);
        }
        if (this.health <= 0)
        {
            this.dead = true;
            //this.gameObject.tag = "DeadUnit";
            
        }
    }
}
