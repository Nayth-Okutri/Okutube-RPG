using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitAction : MonoBehaviour
{
    [SerializeField]
    private GameObject physicalAttack;

    [SerializeField]
    private GameObject magicalAttack;

    private GameObject currentAttack;
    public AudioClip HitFXClip;
    public AudioClip AttackFXClip;
    AudioSource audioSource;

    void Awake()
    {
        this.physicalAttack = Instantiate(this.physicalAttack, this.transform) as GameObject;
        //this.magicalAttack = Instantiate(this.magicalAttack, this.transform) as GameObject;

        this.physicalAttack.GetComponent<AttackTarget>().owner = this.gameObject;
        //this.magicalAttack.GetComponent<AttackTarget>().owner = this.gameObject;

        this.currentAttack = this.physicalAttack;
        audioSource = GetComponent<AudioSource>();
    }

    public void selectAttack(bool physical)
    {
        GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");
        

        this.currentAttack = (physical) ? this.physicalAttack : this.magicalAttack;
        this.currentAttack.GetComponent<AttackTarget>().hit(enemyUnits[0]);
       

    }
    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(AttackFXClip);
    }
    public void PlayHitSound()
    {
        audioSource.PlayOneShot(HitFXClip);
    }
    public void act(GameObject target)
    {
        Debug.Log("Player start to act");
        this.currentAttack.GetComponent<AttackTarget>().hit(target);
    }

    public void EndOfAttack()
    {
        Debug.Log("callback and attack");
        GameObject BattleTurnSystem = GameObject.Find("TurnSystem");
        BattleTurnSystem.GetComponent<BattleTurnSystem>().SetEndTurn();
    }
}


