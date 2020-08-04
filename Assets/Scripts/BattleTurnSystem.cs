using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;



public class BattleTurnSystem : MonoBehaviour
{
    private List<UnitStats> unitsStats;
    private GameObject playerParty;
    int count;
    private State state;
    GameObject VictoryScreen;
    [SerializeField]
    private GameEvent OnPlayerVictory;
    public bool AutoBattle;
    public AudioClip VictoryMusic;
    [SerializeField]
    private GameEvent OnBattleEnd;
    private enum State
    {
        Busy,
        WaitingForPlayer,
        EndTurn,
        WinningScreen,
    }
    // Start is called before the first frame update
    public bool IsWaitingForPlayerInput()
    {
        return state == State.WaitingForPlayer;
    }
    public void SetToBusy()
    {
        state = State.Busy;
    }


    void Start()
    {
        this.playerParty = GameObject.Find("PlayerParty");
        state = State.Busy;
        unitsStats = new List<UnitStats>();
        GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("PlayerUnit");
        foreach (GameObject playerUnit in playerUnits)
        {
            Debug.Log("found Player unit");
            UnitStats currentUnitStats = playerUnit.GetComponent<UnitStats>();
            currentUnitStats.calculateNextActTurn(0);
            unitsStats.Add(currentUnitStats);
        }
        GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");

        foreach (GameObject enemyUnit in enemyUnits)
        {
            Debug.Log("found ennemy unit");
            UnitStats currentUnitStats = enemyUnit.GetComponent<UnitStats>();
            currentUnitStats.calculateNextActTurn(0);
            unitsStats.Add(currentUnitStats);
        }

        unitsStats.Sort();
        count = 0;
        VictoryScreen = GameObject.Find("VictoryScreen");
        if (VictoryScreen != null)
        {
            VictoryScreen.SetActive(false);

        }
        StartCoroutine(DelayStartNextTurn());
       
    }
    IEnumerator DelayStartNextTurn()
    {
        yield return new WaitForSeconds(2f);
       
        this.nextTurn();
    }


    public void SetEndTurn()
    {
        state = State.EndTurn;

    }
    private bool BattleOver()
    {
        foreach (UnitStats playerUnit in unitsStats)
        {
            if (playerUnit.IsDead()) { Destroy(playerUnit.gameObject);  return true; }
        }

        return false;
    }

    private void AfterBattle()
    {
        foreach (UnitStats playerUnit in unitsStats)
        {
            if ((playerUnit.tag == "PlayerUnit"))
            {
                playerUnit.VictoryPose();
                playerUnit.UpdateCharacter();
            }
        }



        state = State.WinningScreen;
         
        if (VictoryScreen != null)
        {
            //chelou, quoi faire avec ca ?
            Item loot = ProgressionSystem.Instance.RollItemForCurrentLoot();
            VictoryScreen.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Vous avez recu : " + loot.name;
            Inventory.Instance.AddInventoryItem(loot, 1);
            VictoryScreen.SetActive(true);
            AudioSource audio = GameObject.Find("Audio Source").GetComponent<AudioSource>();
            audio.clip = VictoryMusic;
            audio.loop = false;
            audio.Play();
        }
        OnPlayerVictory.Raise();

    }

    public void SetAutoBattleOn()
    {
        AutoBattle = true;

    }
    public void nextTurn()
    {
        UnitStats currentUnitStats = unitsStats[0];
        GameObject currentUnit = currentUnitStats.gameObject;

        
        if ((currentUnit.tag == "PlayerUnit"))
        {
            UpdateTurnList();
        }

        count++;

        if ((BattleOver()) || (count >100))
        {
            AfterBattle();
            return;
        }
        if (!currentUnitStats.IsDead())
        {
        
            if (currentUnit.tag == "PlayerUnit")
            {
                Debug.Log("Player unit acting");
                //SKIP
                if (currentUnit.gameObject.name == "Ana") { UpdateTurnList(); return;  }
                state = State.WaitingForPlayer;
                this.playerParty.GetComponent<SelectUnit>().selectCurrentUnit(currentUnit.gameObject);
                if (AutoBattle)
                {
                    //SetToBusy();
                    playerParty.GetComponent<SelectUnit>().selectAttack(true);

                }
            }
            else
            {
                Debug.Log("Enemy unit acting");
                currentUnit.GetComponent<EnemyUnitAction>().act();
                UpdateTurnList();
                this.nextTurn();
            }
         
        }
        else
        {
            //this.nextTurn();
        }
       
    }
    private void UpdateTurnList()
    {
        UnitStats currentUnitStats = unitsStats[0];
        unitsStats.Remove(currentUnitStats);

        currentUnitStats.calculateNextActTurn(currentUnitStats.nextActTurn);
        unitsStats.Add(currentUnitStats);
        unitsStats.Sort();
    }

    // Update is called once per frame


    void Update()
    {
        if (state == State.WinningScreen) 
        {
            if ((Input.GetKeyDown(KeyCode.Escape)))
            {
                OnBattleEnd.Raise();
                //SceneManager.LoadScene("MainAtelier");
            }
            //if (AutoBattle) SceneManager.LoadScene("MainAtelier");
        }
        else if ((state!=State.WaitingForPlayer)&&(state != State.Busy))
        {
            Debug.Log("Player done playin next");
            this.nextTurn();
        }
        if ((AutoBattle) && (state != State.WinningScreen))
        {
            AfterBattle();
        }
    }




    private bool IsCurrentlyPlaying()
    {
        UnitStats currentUnitStats = unitsStats[0];
        if (currentUnitStats.gameObject.tag == "PlayerUnit")
        {
            //if (currentUnitStats.IsStateWaitingForPlayer() || (currentUnitStats.IsStateBusy()))
            {

                return true;
            }

        }
        return false;
    }
}
