using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CampfireManager : MonoBehaviour
{
    public static CampfireManager instance { get; set; }

    public Button MasakButton;
    public Button ExitButton;

    public GameObject makananaSlot;
    public GameObject BahanBakarSlot;

    public GameObject campfirePanel;
    public bool isUiOpenl;

    public Campfire selectedCampfire;
    public CookingData cookingData;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (BahanBakarDanMakananValid())
        {
            MasakButton.interactable = true;
        }
        else { 
            MasakButton.interactable=false;
        }
    }

    private bool BahanBakarDanMakananValid()
    {
        InventoryItem BahanBakar = BahanBakarSlot.GetComponentInChildren<InventoryItem>();
        InventoryItem makanan = makananaSlot.GetComponentInChildren<InventoryItem>();

        

        if (BahanBakar != null && makanan != null) {
            if (cookingData.BahanBakarValid.Contains(BahanBakar.thisName) &&
                cookingData.makananValid.Any(CookableFood => CookableFood.name == makanan.thisName))
            {
                return true;
            }
            else { 
                return false;
            }
        }return false;
    }

    public void TombolMasak() {
        InventoryItem makanan = makananaSlot.GetComponentInChildren<InventoryItem>();
        selectedCampfire.mulaiMasak(makanan);

        InventoryItem BahanBakar = BahanBakarSlot.GetComponentInChildren<InventoryItem>();
        Destroy(makanan.gameObject);
        Destroy(BahanBakar.gameObject);

        closeUi();
    }

    public void OpenUi() {
        campfirePanel.SetActive(true);
        isUiOpenl = true;
        movementManager.instance.aktifkanBergerak(false);
        movementManager.instance.aktifkaanMelihat(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SelectionManager.instance.DisableSelection();
        SelectionManager.instance.GetComponent<SelectionManager>().enabled = false;
        InventorySystem.Instance.OpenUI();
    }

    public void closeUi() {
        campfirePanel.SetActive(false);
        isUiOpenl = false;
        movementManager.instance.aktifkanBergerak(true);
        movementManager.instance.aktifkaanMelihat(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.instance.DisableSelection();
        SelectionManager.instance.GetComponent<SelectionManager>().enabled = true;
    }
}

