using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpeakerUI : MonoBehaviour
{
    public Image Portrait;
    public GameObject CharacterName;
    public GameObject dialog;

    private CharacterPortrait speaker;
    public CharacterPortrait Speaker
    {
        get { return speaker; }
        set
        {
            speaker = value;
            Portrait.sprite = speaker.Portrait;
            CharacterName.GetComponent<TMPro.TextMeshProUGUI>().SetText(speaker.name);
        }
    }
    public string Dialog
    {
        set { dialog.GetComponent<TMPro.TextMeshProUGUI>().SetText(value); }
    }
    public bool HasSpeaker()
    {
        return speaker != null;

    }
    public void Show()
    {
        gameObject.SetActive(true);
        if (Portrait.sprite == null) Portrait.gameObject.SetActive(false);

    }
    public void Hide()
    {

        gameObject.SetActive(false);
    }
    public bool SpeakerIs(CharacterPortrait character)
    {
        return character == speaker;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
