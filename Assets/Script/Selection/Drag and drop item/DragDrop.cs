using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;
    public static bool isBeingDeleted = false;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        //So the ray cast will ignore the item itself.
        canvasGroup.blocksRaycasts = false;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(transform.root);
        itemBeingDragged = gameObject;
    }
    public void OnDrag(PointerEventData eventData)
    {
        //So the item will move with our mouse (at same speed)  and so it will be consistant if the canvas has a different scale (other then 1);
        rectTransform.anchoredPosition += eventData.delta;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        var temppItemReference = itemBeingDragged;
        itemBeingDragged = null;

        //drag item keluar inventory
        if (temppItemReference.transform.parent == temppItemReference.transform.root) {
            
            //meneyembunyikan ikon item 
            temppItemReference.SetActive(false);
            AlertDialogManager dialogManager = FindObjectOfType<AlertDialogManager>();
            dialogManager.ShowDialog("Apakah kamu ingin drop item?", (response) => {
                if (response)
                {
                    dropItemKeGround(temppItemReference);
                }
                else
                {
                    BatalDragItem(temppItemReference);
                }
            });
        }

        //drag di slot yang sama
        if (temppItemReference.transform.parent == startParent) {
            BatalDragItem(temppItemReference);

        }
        //drop di slot yang lain 
        if (temppItemReference.transform.parent != temppItemReference.transform.root &&
            temppItemReference.transform.parent != startParent) {

            //slot tidak menerima item yang sama atau limit stack
            if (temppItemReference.transform.parent.childCount > 2) {
                BatalDragItem(temppItemReference);

                Debug.Log("tidak diterima dislot ini");
            } else { //item pindah ke slot yang lain 

                if (Input.GetKey(KeyCode.LeftShift)) {
                    MembagiStack(temppItemReference);
                
                }
                Debug.Log("menerima slot");
            
            }
        }
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    private void MembagiStack(GameObject temppItemReference)
    {
        InventoryItem item = temppItemReference.GetComponent<InventoryItem>();
        //cek jika item/ stack berpindah 1 item 
        if (item.jumlahDalamInventory > 1) {
            item.jumlahDalamInventory -= 1;
            InventorySystem.Instance.AddToInventory(item.thisName, false); // false karena tidak ingin stack
        }
    }

    void BatalDragItem(GameObject temppItemReference) {
        transform.position = startPosition;
        transform.SetParent(startParent);

        temppItemReference.SetActive(true);

    }

    private void dropItemKeGround(GameObject tempItemReference)
    {
        string cleanName = tempItemReference.name.Split(new string[] { "(Clone)" }, StringSplitOptions.None)[0];

        GameObject item = Instantiate(Resources.Load<GameObject>(cleanName + "_Model"));

        item.transform.position = Vector3.zero;
        var dropSpawnPosition = PlayerState.Instance.playerBody.transform.Find("DropSpawn").transform.position;
        item.transform.localPosition = new Vector3(dropSpawnPosition.x, dropSpawnPosition.y, dropSpawnPosition.z);

        var itemsObject = FindObjectOfType<EnviromentManager>().gameObject.transform.Find("item");
        item.transform.SetParent(itemsObject.transform);

        DestroyImmediate(tempItemReference.gameObject);
        InventorySystem.Instance.ReCalculateList();
        CraftingSystem.instance.RefreshNeededItems();
    }
}