using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HackGetout : MonoBehaviour
{
    // Start is called before the first frame update
    float timerDisplay;
    void Start()
    {
        timerDisplay = 5f;
    }
    public void ForceChangeSceneToWorldMap()
    {
        StartCoroutine(LoadLevel("Worldmap"));

    }
  

    IEnumerator LoadLevel(string scene)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
    }
    // Update is called once per frame
    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                ForceChangeSceneToWorldMap();
            }
        }
    }
}
