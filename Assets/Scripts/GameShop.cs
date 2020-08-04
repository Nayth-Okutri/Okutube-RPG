using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameShop : MonoBehaviour
{
    public ShopStock ItemStock;
    public GameObject ShopUI;
    public GameObject ItemList;
    public GameObject ItemElement;
    public Transform ItemDetails;
    public float stepSize = 1;
    private bool Initialized = false;
    //TEMP
    public GameObject OKButton;
    private int CurrentItemIndex;
    // Start is called before the first frame update
    void Start()
    {
        HideShopMenu();
        ItemDetails.GetChild(0).gameObject.SetActive(false);
        ItemElement.SetActive(false);
    }

    IEnumerator ShowMenuContent()
    {
        yield return new WaitForSeconds(1f);
        //InventoryUI shopUI = TheShopInterface.GetComponent<InventoryUI>();
        //shopUI.RefreshShopMenu(ItemStock.AvailableItems);
        //RewardLabel.GetComponent<TMPro.TextMeshProUGUI>().SetText(ActivityManager.Instance.GetCurrentActivityDescription());
    }

    public void RefreshItemSelection()
    {
        var cursor = ItemElement.transform.localPosition;
        for (int i = 0; i < ItemStock.AvailableItems.Count; i++)
        {
            int j = i;
            var e = Instantiate(ItemElement);
            e.gameObject.SetActive(true);
            e.transform.SetParent(ItemList.transform);
            e.transform.localPosition = cursor;
            e.transform.localScale = new Vector3(10f, 10f, 10f);
            Transform ItemNode = ItemList.transform.GetChild(i).transform;
            e.transform.GetChild(0).GetComponent<Image>().sprite = ItemStock.AvailableItems[i].icon;
            e.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => { ShowItemDetails(j); });
            e.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = ItemStock.AvailableItems[i].fullname;
            
            cursor.y -= stepSize;
        }

        Initialized = true;

    }


    public void BuyItem()
    {
        if (Inventory.Instance != null)
            if (Inventory.Instance.GetMoneyCount() > ItemStock.PriceList[CurrentItemIndex])
            {
                Inventory.Instance.UpdateMoneyCount(-ItemStock.PriceList[CurrentItemIndex]);
                Inventory.Instance.AddInventoryItem(ItemStock.AvailableItems[CurrentItemIndex], 1);
                UpdateOwnedObject(CurrentItemIndex);
            }
               
    }

    //A SEPARER
    public void DisplayShopMenu()
    {
        if (ShopUI != null)
        {
            ShopUI.SetActive(true);
            OKButton.SetActive(true);
            Animator PanelMove = ShopUI.GetComponent<Animator>();
            PanelMove.SetBool("IsHidden", false);
            if (!Initialized) RefreshItemSelection();
        }

    }

    public void ShowItemDetails(int i)
    {
        CurrentItemIndex = i;
        ItemDetails.GetChild(0).gameObject.SetActive(true);
        ItemDetails.GetChild(0).GetComponent<Image>().sprite = ItemStock.AvailableItems[i].icon;
        ItemDetails.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = ItemStock.AvailableItems[i].name;
        ItemDetails.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = ItemStock.AvailableItems[i].Description;
        ItemDetails.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = "Price : " + ItemStock.PriceList[i].ToString();
        UpdateOwnedObject(i);
    }

    public void UpdateOwnedObject(int i)
    {

        ItemDetails.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = "Vous avez : " + Inventory.Instance.GetInventoryCount(ItemStock.AvailableItems[i]).ToString();
    }

    public void HideShopMenu()
    {
        Animator PanelMove = ShopUI.GetComponent<Animator>();
        PanelMove.SetBool("IsHidden", true);
       
    }
        public void BuyFirstObject()
    {

        Inventory.Instance.AddInventoryItem(ItemStock.AvailableItems[0], 1);
    }
    public void BuyObject2()
    {

        Inventory.Instance.AddInventoryItem(ItemStock.AvailableItems[1], 1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
