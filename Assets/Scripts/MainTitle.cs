using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainTitle : MonoBehaviour
{
    public AudioClip OkutriFXClip;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayLogoSound()
    {
        audioSource.PlayOneShot(OkutriFXClip);
    }
    public void GoToNext()
    {
        SceneManager.LoadScene("MainAtelier");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            {
                GoToNext();
            }
            
        }
    }
}
