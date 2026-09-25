using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static BuySystem;

public class ShopItemSlot : MonoBehaviour
{
    [Header("UI")]
    public Image itemImageUI;
    public TextMeshProUGUI itemNameUI;
    public TextMeshProUGUI itemPriceUI;
    public Button buyButtonUI;

    [Header("Data")]
    public ShopItemData shopItemData;

    private void Start()
    {
        buyButtonUI.onClick.AddListener(BuyItem);
    }

    // Update is called once per frame
    void Update()
    {
       
        int currentMoney = InventorySystem.Instance.uangSekarang;

        // Enable/Disable Button based on current owned money
        if (shopItemData.itemPrice <= currentMoney) 
        {
            buyButtonUI.interactable = true;
        }
        else
        {
            buyButtonUI.interactable = false;
        }
    }

    public void BuyItem()
    {
        // Remove Coins
        InventorySystem.Instance.uangSekarang -= shopItemData.itemPrice;

        // Get inventory item Name
        InventoryItem inventoryItem = shopItemData.inventoryItem.GetComponent<InventoryItem>();

        // Add items into inventory
        InventorySystem.Instance.AddToInventory(inventoryItem.thisName, true);

        Debug.Log("Bought " + inventoryItem.thisName);
    }
}
