using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;
    public List<GameObject> quickSlotsList = new List<GameObject>();

    public GameObject numbersHolder;

    public int selectedNumber = -1;
    public GameObject selectedItem;

    public GameObject toolHolder;

    public GameObject selectedItemModel;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        PopulateSlotList();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectQuickSlot(1);

        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectQuickSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectQuickSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectQuickSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectQuickSlot(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            selectQuickSlot(6);
        }
    }

    void selectQuickSlot(int number) {

        if (checkIfSlotIsFull(number) == true) {

            if (selectedNumber != number)
            {
                selectedNumber = number;
                //membatalkan seleksi item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }
                selectedItem = getSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;

                setEquippedModel(selectedItem);
            }
            else { //seleksi slot yg sama
                selectedNumber = -1;
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }
                if (selectedItemModel != null) { //melepas item yang dipegang
                    DestroyImmediate(selectedItemModel.gameObject);
                    selectedItemModel = null;
                
                }
            }
        }
    }
    GameObject getSelectedItem(int slotNumber) { 
        return quickSlotsList[slotNumber-1].transform.GetChild(0).gameObject;
    }

    private void setEquippedModel(GameObject selectedItem) {

        if (selectedItemModel != null) //melepas item yang dipegang
        {
            DestroyImmediate(selectedItemModel.gameObject);
            selectedItemModel = null;
        }

        string selectedItemName = selectedItem.name.Replace("(Clone)", "");
        selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"),
            new Vector3(0.6f, 0.45f, 0.58f), Quaternion.Euler(55, 188f, 101.2f));
        selectedItemModel.transform.SetParent(toolHolder.transform, false);
    }


    bool checkIfSlotIsFull(int slotNumber) {
        if (quickSlotsList[slotNumber-1].transform.childCount > 0) {
            return true;

        }else { 
            return false;
        }
    }
    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);
        InventorySystem.Instance.ReCalculateList();
    }

    public GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {

        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }
        if (counter == 6)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    internal int weaponDamage()
    {
        if (selectedItem != null)
        {
            return selectedItem.GetComponent<Weapon>().weaponDmg;
        }
        else
        {
            return 0;
        }
    }

    internal bool memegangSenjata()
    {
        if (selectedItem != null) {
            if (selectedItem.GetComponent<Weapon>() != null)
            {
                return true;
            }
            else { 
                return false;
            }
        }
        else {
            return false;
        }
    }

    internal bool IsThereASwingLock()
    {
        if (selectedItem && selectedItemModel.GetComponent<EquipableItem>()){
            return selectedItemModel.GetComponent<EquipableItem>().swingwait;

        }
        else {

            return false;
        }
    }
}