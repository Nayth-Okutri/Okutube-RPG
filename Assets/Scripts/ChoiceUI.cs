using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceUI : MonoBehaviour
{
    public GameObject dialog;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public string Choice
    {
        set { dialog.GetComponent<TMPro.TextMeshProUGUI>().SetText(value); }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
