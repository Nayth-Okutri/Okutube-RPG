using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShopStock : ScriptableObject
{
    public List<Item> AvailableItems = new List<Item>();
    public List<int> PriceList = new List<int>();
    public Dictionary<Item, int> ItemStockToSell = new Dictionary<Item, int>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Item item in AvailableItems)
            ItemStockToSell.Add(item, 20);
    }

   
}
