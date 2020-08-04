using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public int count = 1;
    [SerializeField]
    private Item item;
    public bool RandomAmount;
    public int MaxRdmAmount;
    void OnEnable()
    {
        GetComponent<SpriteRenderer>().sprite = item.icon;
        if (RandomAmount)
        {
            count = Random.Range(1, MaxRdmAmount);
        }

    }
    public void GetItem()
    {
      
        gameObject.SetActive(false);
        Inventory.Instance.AddInventoryItem(item, count);
       
    }

}


