using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public TextMeshProUGUI JumlahTXT;
    public InventoryItem itemInSlot;

    private void Update()
    {
        InventoryItem item = CekInventoryItem();
        if (item != null)
        {
            itemInSlot = item;
        }
        else { 
            itemInSlot = null;
        }

        if (itemInSlot != null)
        {
            JumlahTXT.gameObject.SetActive(true);
            JumlahTXT.text = $"{itemInSlot.jumlahDalamInventory}";
            JumlahTXT.transform.SetAsLastSibling();
        }
        else {
            JumlahTXT.gameObject.SetActive(false);//text didepan item
        }
    }
    private InventoryItem CekInventoryItem()
    {
        foreach(Transform child in transform){
            if (child.GetComponent<InventoryItem>()) { 
                return child.GetComponent<InventoryItem>();
            }
        }
        return null;
    }

    public void UpdateItemInslot() {
        itemInSlot = CekInventoryItem();
    }
}
