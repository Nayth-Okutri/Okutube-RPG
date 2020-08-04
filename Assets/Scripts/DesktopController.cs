using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class DesktopController : MonoBehaviour
{
    public GameObject MenuItem;
    public GameObject TextMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainAtelier");

        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene("Worldmap");

        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            if( MenuItem != null)
            {
                MenuItem.SetActive(true);
                TextMenu.SetActive(true);
                // GetComponent<TextMenu>();
                // MenuItem. 

            }
        }
    }
}
