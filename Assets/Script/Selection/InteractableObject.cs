using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public string ItemName;

    public string GetItemName()
    {
        return ItemName;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerInRange && SelectionManager.instance.onTarget&& SelectionManager.instance.selectedObject == gameObject) {
            //jika inventory tidak penuh
            if (InventorySystem.Instance.CheckIfFull(1))
            {
                InventorySystem.Instance.AddToInventory(ItemName, true); //true
                InventorySystem.Instance.itemPickUp.Add(gameObject.name);
                
                Destroy(gameObject);
            }
            else {
                Debug.Log("inventory is full");
            
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) { 
            playerInRange = true;
            
        }
    
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            
        }
    }
}
