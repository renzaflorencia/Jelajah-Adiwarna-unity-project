using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class sellSystem : MonoBehaviour
{
    #region || -- Singelton -- ||
    public static sellSystem Instance { get; set; }

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
    #endregion

    public Button sellBTN;
    public TextMeshProUGUI jumlahJualTXT;
    public Button backBTN;

    public List<InventorySlot> slotJual;
    public List<InventoryItem> ItemsYangDijual;
    public GameObject panelJual;

    [Header("penjual")]
    public penjual Penjual;

    private void Start()
    {
        GetAllSlot();

        sellBTN.onClick.AddListener(JualItem);
        backBTN.onClick.AddListener(keluarModeJual);
    }

    private void JualItem() {
        List<GameObject> itemToDestroy = new List<GameObject>();

        int UangYangDiperoleh = 0;
        foreach (InventoryItem item in ItemsYangDijual) { 
            itemToDestroy.Add(item.gameObject);
            UangYangDiperoleh += (item.jumlahDalamInventory * item.hargaJual);

        }
        InventorySystem.Instance.uangSekarang += UangYangDiperoleh;

        foreach (GameObject ob in itemToDestroy) { 
            Destroy(ob);
        }
        itemToDestroy.Clear();
        ItemsYangDijual.Clear();

        upateJumlahJualUI();
    }

    private void GetAllSlot()
    {
        slotJual.Clear();
        foreach (Transform child in panelJual.transform) {
            if (child.CompareTag("Slot")) {
                slotJual.Add(child.GetComponent<InventorySlot>());
            }
        }
    }

    public void scanItemsInSlot() {
        ItemsYangDijual.Clear();
        foreach (InventorySlot slot in slotJual)
        {
            if (slot.itemInSlot != null)
            {
                ItemsYangDijual.Add(slot.itemInSlot);
            }
        }
    }

    public void upateJumlahJualUI() {
        int JumlahTotaldiDisplay = 0;
        foreach (InventoryItem item in ItemsYangDijual) {
            JumlahTotaldiDisplay += (item.jumlahDalamInventory * item.hargaJual);
        }
        jumlahJualTXT.text = JumlahTotaldiDisplay.ToString();
    }

    private void keluarModeJual()
    {
        if (sellPanelEmpty()) {
            Penjual.modeDialog();
        }
    }

    private bool sellPanelEmpty()
    {
        if (ItemsYangDijual.Count <= 0)
        {
            return true;

        }
        else { 
            return false;
        }
    }
}
