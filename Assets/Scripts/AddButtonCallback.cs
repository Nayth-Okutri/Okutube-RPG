using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AddButtonCallback : MonoBehaviour
{
    [SerializeField]
    private bool physical;
    public bool exitButton;
    // Use this for initialization
    void Start()
    {
        physical = true;
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => addCallback());
    }

    private void addCallback()
    {
        GameObject playerParty = GameObject.Find("PlayerParty");
        if (exitButton)
        {
            SceneManager.LoadScene("MainAtelier");

        }
        if (playerParty != null)
        {
            playerParty.GetComponent<SelectUnit>().selectAttack(this.physical);
        }
        
    }
}
