using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : MonoBehaviour
{
    public GameObject owner;

    [SerializeField]
    private string attackAnimation;

    [SerializeField]
    private bool magicAttack;

    [SerializeField]
    private float manaCost;

    [SerializeField]
    private float minAttackMultiplier;

    [SerializeField]
    private float maxAttackMultiplier;

    [SerializeField]
    private float minDefenseMultiplier;

    [SerializeField]
    private float maxDefenseMultiplier;

    private Animator animator;


    [SerializeField]
    private GameEvent EnemyHitEvent;
    [SerializeField]
    private GameEvent PlayerHitEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void hit(GameObject target)
    {
        UnitStats ownerStats = this.owner.GetComponent<UnitStats>();
        UnitStats targetStats = target.GetComponent<UnitStats>();
        animator = this.owner.GetComponent<Animator>();
      
        if (ownerStats.mana >= this.manaCost)
        {
            
            Debug.Log(ownerStats.name + " ATTACK");
       
            float attackMultiplier = (Random.value * (this.maxAttackMultiplier - this.minAttackMultiplier)) + this.minAttackMultiplier;
            float damage = (this.magicAttack) ? attackMultiplier * ownerStats.magic : attackMultiplier * ownerStats.attack;

            float defenseMultiplier = (Random.value * (this.maxDefenseMultiplier - this.minDefenseMultiplier)) + this.minDefenseMultiplier;
            damage = Mathf.Max(0, damage - (defenseMultiplier * targetStats.defense));
            damage = (int)damage;
            this.owner.GetComponent<Animator>().Play(this.attackAnimation);

            targetStats.receiveDamage(damage);

            if ((targetStats.gameObject.name =="Ennemy_Doute") &&(EnemyHitEvent != null))
            {
                EnemyHitEvent.sentFloat = targetStats.health/ targetStats.maxHealth;
                EnemyHitEvent.Raise();
            }
           else if (PlayerHitEvent != null)
            {
                PlayerHitEvent.sentFloat = targetStats.health;
                PlayerHitEvent.Raise();
            }
            ownerStats.mana -= this.manaCost;
        }
    }


   

    IEnumerator waiting(float time)
    {
        yield return new WaitForSecondsRealtime(time);
    }


}
