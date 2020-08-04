using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    public Transform elementPrototype;
    public float stepSize = 1;

    Vector2 firstItem;
    // Start is called before the first frame update
    void Start()
    {
        firstItem = elementPrototype.localPosition;
        elementPrototype.gameObject.SetActive(false);
        //Refresh();
    }

    public void Refresh()
    {
        if (Inventory.Instance != null)
        {
            var cursor = firstItem;
            for (var i = 1; i < transform.childCount; i++)
                Destroy(transform.GetChild(i).gameObject);
            var displayCount = 0;

            for (int i =0; i< Inventory.Instance.GetTotalInventoryCount();i++)
            {
                //Tout sauf gold ...
                string ItemName = Inventory.Instance.GetInventoryItemNameAtRank(i);
                if (ItemName != "MaiMoney")
                {
                    var count = Inventory.Instance.GetItemCountByName(ItemName);
                    if (count <= 0) continue;
                    displayCount++;
                    var e = Instantiate(elementPrototype);
                    e.transform.SetParent(transform);
                    e.transform.localPosition = cursor;
                    e.transform.localScale = new Vector3(1f, 1f, 1f);
                    e.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.Instance.GetInventorySprite(ItemName);
                    e.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = $"x {count}";
                    e.gameObject.SetActive(true);
                    cursor.y -= stepSize;
                }
                
            }
        }
    }



    public void RefreshShopMenu(List<Item> listItem)
    {
        var cursor = firstItem;
        for (var i = 1; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
        var displayCount = 0;
        int j = 0;
        foreach (var i in listItem)
        {
            //var count = Inventory.Instance.GetInventoryCount(i);
            // if (count <= 0) continue;
            displayCount++;
            int jbis = j;
            var e = Instantiate(elementPrototype);
            e.gameObject.SetActive(true);
            e.transform.SetParent(transform);
            e.transform.localPosition = cursor;
            e.transform.localScale = new Vector3(10f, 10f, 10f);
            e.transform.GetChild(0).GetComponent<Image>().sprite = i.icon;
            e.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = i.name;
            Button but = e.transform.GetChild(0).GetComponent<Button>();
            //but.onClick.AddListener(delegate {SwitchButtonHandler(jbis); });
           //e.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => { var jbis = j; SwitchButtonHandler(jbis); });
            //AddListener(but, jbis);

            
            cursor.y -= stepSize;
            j++;
        }
        //for (int i = 0; i < listItem.Count;i++)
        {
            //transform.GetChild(0).GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => {  SwitchButtonHandler(0); });
            transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() => { SwitchButtonHandler(0); });
            transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => { SwitchButtonHandler(1); });
            transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => { SwitchButtonHandler(2); });
        }
        
    }
    void AddListener(Button b, int i)
    {
        b.onClick.AddListener(() => SwitchButtonHandler(i));
    }
    void SwitchButtonHandler(int i)
    {
        Debug.Log("lol : " + i);
    }
}
