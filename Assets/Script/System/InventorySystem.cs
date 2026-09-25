using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public GameObject ItemInfoUi;
    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;

    public List<InventorySlot>slotList = new List<InventorySlot>();
    public List<string>itemList = new List<string>();
    private GameObject itemToAdd;
    private InventorySlot whatSlotToEquip;
    public bool isOpen;
    internal int uangSekarang = 100;
    public TextMeshProUGUI UiUang;
    //public bool isFull;

    //pick up pop up
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;

    public List<string> itemPickUp;
    public int stackLimit = 10;

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
    void Start()
    {
        isOpen = false;
        movementManager.instance.aktifkanBergerak(true);
        movementManager.instance.aktifkaanMelihat(true);
        PopulateSlotList();
        Cursor.visible = false;
    }

    private void PopulateSlotList() {

        foreach (Transform child in inventoryScreenUI.transform) { 
            if(child.CompareTag("Slot")){
                InventorySlot slot = child.GetComponent<InventorySlot>();
                slotList.Add(slot);
            }
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {
           OpenUI();

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            closeUI();
        }

        UiUang.text =$"{uangSekarang} koin";
    }

    public void OpenUI() {
        inventoryScreenUI.SetActive(true);

        inventoryScreenUI.GetComponentInParent<Canvas>().sortingOrder = MenuManager.Instance.setAsFront();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SelectionManager.instance.DisableSelection();
        SelectionManager.instance.GetComponent<SelectionManager>().enabled = false;

        movementManager.instance.aktifkanBergerak(false);
        movementManager.instance.aktifkaanMelihat(false);

        isOpen = true;
      
        ReCalculateList();
    }

    public void closeUI() {
        inventoryScreenUI.SetActive(false);

        if (!CraftingSystem.instance.isOpen && !StorageManager.Instance.storageUIOpen && 
            !CampfireManager.instance.isUiOpenl && 
            !BuySystem.Instance.Penjual.berbicaraDenganPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SelectionManager.instance.EnableSelection();
            SelectionManager.instance.GetComponent<SelectionManager>().enabled = true;

            movementManager.instance.aktifkanBergerak(true);
            movementManager.instance.aktifkaanMelihat(true);

        }

        isOpen = false;
     
    }

    public void AddToInventory(string itemName , bool shouldStack) {


        InventorySlot stack = cekStackJikaAda(itemName);
        if (stack != null && shouldStack)
        {
            stack.itemInSlot.jumlahDalamInventory += 1;
            stack.UpdateItemInslot();
        }
        else {
            whatSlotToEquip = FindNextEmptySlot();
            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform);
            itemToAdd.transform.localPosition = Vector3.zero;
            itemToAdd.transform.localScale = Vector3.one; // kalau ukuran aneh


            itemToAdd.transform.SetParent(whatSlotToEquip.transform, false);

            itemList.Add(itemName);
        }
        SoundSystem.instance.PlaySound(SoundSystem.instance.pickUpSound);
        TriggerPickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);

        ReCalculateList();
        CraftingSystem.instance.RefreshNeededItems();
        QuestManager.Instance.refreshTrackerList();
    }

    private InventorySlot cekStackJikaAda(string itemName)
    {
        foreach (InventorySlot inventorySlot in slotList) {
            inventorySlot.UpdateItemInslot();

            if (inventorySlot != null && inventorySlot.itemInSlot != null) {
                if (inventorySlot.itemInSlot.thisName == itemName &&
                    inventorySlot.itemInSlot.jumlahDalamInventory < stackLimit) { 
                    return inventorySlot;
                }
            }
        }
        return null;
    }

    void TriggerPickupPopUp(string itemName, Sprite itemSprite ) {
        pickupAlert.SetActive(true);
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;
        StartCoroutine(HidePickupAlertAfterDelay(2f)); //hide popup

    }
    private IEnumerator HidePickupAlertAfterDelay(float delay) //hide popup
    {
        yield return new WaitForSeconds(delay);
        pickupAlert.SetActive(false);
    }

    private InventorySlot FindNextEmptySlot() {
        foreach (InventorySlot slot in slotList) {
            if (slot.transform.childCount <= 1) {
                return slot;
            
            }
        }
        return null;
    }

    public bool CheckIfFull(int emptyMeeded) {
        int CheckIfFull = 0;
        foreach (InventorySlot slot in slotList) {
            if (slot.transform.childCount <= 1) {
                CheckIfFull += 1;
            }
           
        }
        if (CheckIfFull >= emptyMeeded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveItem(string itemName, int jumlahDihapus) {
        int JumlahTersisaDihilangkan = jumlahDihapus;

        while (JumlahTersisaDihilangkan != 0) {
            int jumlahSisaSebelumnya = JumlahTersisaDihilangkan;

            foreach (InventorySlot slot in slotList) {
                if (slot.itemInSlot != null && slot.itemInSlot.thisName == itemName) {
                    slot.itemInSlot.jumlahDalamInventory--;
                    JumlahTersisaDihilangkan--;

                    if (slot.itemInSlot.jumlahDalamInventory == 0) {
                        Destroy(slot.itemInSlot.gameObject);
                        slot.itemInSlot = null;
                    }
                    break; //keluar dari loop jika item ditemukan dan dihapus 
                }
            }

            //cek 
            if (jumlahSisaSebelumnya == JumlahTersisaDihilangkan) {
                Debug.Log("item tidak ditemukan atau tidak memandai");
                break;
            
            }

            ReCalculateList();
            CraftingSystem.instance.RefreshNeededItems();
            QuestManager.Instance.refreshTrackerList();
        }
    }

    public void ReCalculateList() { 
       itemList.Clear();

        foreach (InventorySlot inventorySlot in slotList) {
            InventoryItem item = inventorySlot.itemInSlot;
            if (item != null) {
                if (item.jumlahDalamInventory > 0)
                {
                    for (int i = 0; i < item.jumlahDalamInventory; i++)
                    {
                        itemList.Add(item.thisName);
                    }
                }

            }
        }
    }

    public int CheckJumlahItem(string name) { 
        int itemCounter = 0 ;
        foreach (string item in itemList) {
            if (item == name) { 
                itemCounter++;
            }
        }
        return itemCounter;

    }
}