using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{

    //public GameObject Item
    //{
    //    get
    //    {
    //        if (transform.childCount > 0)
    //        {
    //            return transform.GetChild(0).gameObject;
    //        }

    //        return null;
    //    }
    //}

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");

        //slot kosong
        if (transform.childCount <= 1)
        {
            SoundSystem.instance.PlaySound(SoundSystem.instance.dropItemSound);

            DragDrop.itemBeingDragged.transform.SetParent(transform);
            DragDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);

            if (transform.CompareTag("QuickSlot") == false)
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuickSlot = false;
                InventorySystem.Instance.ReCalculateList();
            }
            if (transform.CompareTag("QuickSlot"))
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuickSlot = true;
                InventorySystem.Instance.ReCalculateList();
            }
        }
        else { //slot terisi 
            //name => game objek 
            //thisName => field 
            InventoryItem draggedItem = DragDrop.itemBeingDragged.GetComponent<InventoryItem>();
            if (draggedItem.thisName == GetStoredItem().thisName && isLimit(draggedItem) == false)
            {
                //gabungkan dan simpan item 
                GetStoredItem().jumlahDalamInventory += draggedItem.jumlahDalamInventory;
                DestroyImmediate(DragDrop.itemBeingDragged);
            }
            else {
                DragDrop.itemBeingDragged.transform.SetParent(transform);
            }
        }

        StartCoroutine(delayScan());
    }

    IEnumerator delayScan() {
        yield return new WaitForSeconds(0.1f);
        sellSystem.Instance.scanItemsInSlot();
        sellSystem.Instance.upateJumlahJualUI();
    }

    InventoryItem GetStoredItem() { 
        return transform.GetChild(0).GetComponent<InventoryItem>();  
    }

    bool isLimit(InventoryItem draggedItem) {
        if ((draggedItem.jumlahDalamInventory + GetStoredItem().jumlahDalamInventory) > InventorySystem.Instance.stackLimit)
        {
            return true;

        }
        else { 
            return false;
        }
    }
}