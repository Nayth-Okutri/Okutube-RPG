using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Inventory : MonoBehaviour
{
    Dictionary<Item, int> inventory = new Dictionary<Item, int>();
    Dictionary<string, Sprite> inventorySprites = new Dictionary<string, Sprite>();
    //public IEnumerable<string> InventoryItems => inventory.Keys;
    public static Inventory Instance;
    Dictionary<ConventionItem, int> KeyConventionItems = new Dictionary<ConventionItem, int>();
    [SerializeField]
    private GameEvent OnChangeInventory;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (OnChangeInventory != null) OnChangeInventory.Raise();

    }

    public int GetTotalInventoryCount()
    {
        return inventory.Count;

    }

    public string GetInventoryItemNameAtRank(int theRank)
    {
        int iterator = 0;
        foreach (KeyValuePair<Item, int> i in inventory)
        {
            if (iterator == theRank) return i.Key.fullname;
            iterator++;
        }
        return "";
    }
    public Sprite GetInventorySprite(string name)
    {
        Sprite s;
        // inventorySprites.TryGetValue(name, out s);
        //return s;
        foreach (KeyValuePair<Item, int> i in inventory)
        {
            if (i.Key.fullname == name) return i.Key.icon;
        }
        return null;
    }
    public int GetInventoryCount(Item item)
    {
        int c;
        inventory.TryGetValue(item, out c);
        return c;
    }
    public int GetKeyInventoryCount(ConventionItem item)
    {
        int c;
        KeyConventionItems.TryGetValue(item, out c);
        return c;
    }
    public int GetKeyItemTypeCount(string conventionItemType)
    {
       int c=0;
       foreach (KeyValuePair<ConventionItem, int> i in KeyConventionItems)
       {
            if (i.Key.ItemType.TypeName == conventionItemType) c += i.Value;
       }
       return c;
    }
    public void UpdateMoneyCount(int i)
    {
        /*int c;
        c = GetItemCountByName("MaiMoney")
        //inventory.TryGetValue("MaiMoney", out c);
        c += i;
        inventory["MaiMoney"] = c;*/
        UpdateItemCountByName("MaiMoney", i);

    }

    public int GetMoneyCount()
    {
        int c;
        //inventory.TryGetValue("MaiMoney", out c);
        c = GetItemCountByName("MaiMoney");
        return c;
    }
    public bool RemoveInventoryItem(Item item, int count)
    {
        int c = 0;
        inventory.TryGetValue(item, out c);
        c += count;
        if (c < 0) return false;
        inventory[item] = c;
        if (OnChangeInventory != null) OnChangeInventory.Raise();
        
        return true;
    }

    public void AddInventoryKeyItem(ConventionItem item, int count)
    {
        int c = 0;
        KeyConventionItems.TryGetValue(item, out c);
        c += count;
        KeyConventionItems[item] = c;
        if (OnChangeInventory != null) OnChangeInventory.Raise();
    }
    public void AddInventoryItem(Item item,int count)
    {
        int c = 0;
        inventory.TryGetValue(item, out c);
        c += count;
        inventory[item] = c;
        inventorySprites[item.name] = item.icon;
        if (OnChangeInventory != null) OnChangeInventory.Raise();
    }

    public void UpdateItemCountByName(string itemName, int count)
    {
        //int c = 0;
        foreach (KeyValuePair<Item, int> i in inventory)
        {
            if (i.Key.fullname == itemName)
            {
                inventory[i.Key] += count;
                break;
            }
        }
    }

    public int GetItemCountByName(string itemName)
    {
       
        foreach (KeyValuePair<Item, int> i in inventory)
        {
            if (i.Key.fullname == itemName) return i.Value;
        }
        return 0;

    }

    public bool HasInventoryItem(string name, int count = 1)
    {
        int c = 0;
        c = GetItemCountByName(name);
        //inventory.TryGetValue(name, out c);
        return c >= count;
    }

}
