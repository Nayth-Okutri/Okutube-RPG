using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectUnit : MonoBehaviour
{
    private GameObject currentUnit;

    private GameObject actionsMenu, enemyUnitsMenu;


    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Battle")
        {
            //this.actionsMenu = GameObject.Find("ActionsMenu");
            //this.enemyUnitsMenu = GameObject.Find("EnemyUnitsMenu");
        }
    }

    public void selectCurrentUnit(GameObject unit)
    {
        Debug.Log("Current unit " + unit.name);
        this.currentUnit = unit;
        //this.actionsMenu.SetActive(true);
    }

    public void selectAttack(bool physical)
    {
        BattleTurnSystem TurnSystem = GameObject.Find("TurnSystem").GetComponent<BattleTurnSystem>();
        if (TurnSystem.IsWaitingForPlayerInput())
        {
            TurnSystem.SetToBusy();
            Debug.Log("unit selected " + currentUnit.name);
            this.currentUnit.GetComponent<PlayerUnitAction>().selectAttack(physical);
            //currentUnit.GetComponent<UnitStats>().SetStateEndTurn();
            Debug.Log("DoneActing true");

            //this.actionsMenu.SetActive(false);
            //this.enemyUnitsMenu.SetActive(true);
        }

    }

    public void attackEnemyTarget(GameObject target)
    {
        //this.actionsMenu.SetActive(false);
        //this.enemyUnitsMenu.SetActive(false);
        this.currentUnit.GetComponent<PlayerUnitAction>().act(target);
    }
}
