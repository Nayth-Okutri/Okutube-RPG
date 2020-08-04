using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHealthIndicator : MonoBehaviour
{
    public Image mask;
    public static UIHealthIndicator instance { get; private set; }
    float originalSize;
    float maxHealth;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (mask != null)  originalSize = mask.rectTransform.rect.width;
        maxHealth = 5f;
    }


    public void SetHealth(float value)
    {
        TextMeshProUGUI UIText = GetComponent<TMPro.TextMeshProUGUI>();
            
        if (UIText!=null) UIText.SetText("" + value);

        if (mask != null) SetValue(value);
    }


    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (originalSize * value)/ maxHealth);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
