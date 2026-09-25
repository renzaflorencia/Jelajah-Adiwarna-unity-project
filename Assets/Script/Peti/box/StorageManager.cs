using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance { get; set; }

    [SerializeField] GameObject storageBoxSmallUI;
    [SerializeField] StorageBox selectedStorage;
    public bool storageUIOpen;

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

    public void OpenBox(StorageBox storage)
    {
        SetSelectedStorage(storage);

        PopulateStorage(GetRelevantUI(selectedStorage)); //eror

        GetRelevantUI(selectedStorage).SetActive(true);
        storageUIOpen = true;
        movementManager.instance.aktifkanBergerak(false);
        movementManager.instance.aktifkaanMelihat(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SelectionManager.instance.DisableSelection();
        SelectionManager.instance.GetComponent<SelectionManager>().enabled = false;
    }

    private void PopulateStorage(GameObject storageUI)
    {
        // Get all slots of the ui
        List<GameObject> uiSlots = new List<GameObject>();

        foreach (Transform child in storageUI.transform)
        {
            uiSlots.Add(child.gameObject);
        }

        // Now, instantiate the prefab and set it as a child of each GameObject
        foreach (string name in selectedStorage.items)
        {

            foreach (GameObject slot in uiSlots)
            {
                if (slot.transform.childCount < 1)
                {
                    var itemToAdd = Instantiate(Resources.Load<GameObject>(name), slot.transform.position, slot.transform.rotation);

                    itemToAdd.name = name;
                    itemToAdd.transform.SetParent(slot.transform);
                    break;
                }
            }
        }
    }

    public void CloseBox()
    {
        RecalculateStorage(GetRelevantUI(selectedStorage));

        GetRelevantUI(selectedStorage).SetActive(false);
        storageUIOpen = false;
        movementManager.instance.aktifkanBergerak(true);
        movementManager.instance.aktifkaanMelihat(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.instance.EnableSelection();
        SelectionManager.instance.GetComponent<SelectionManager>().enabled = true;
    }

    private void RecalculateStorage(GameObject storageUI)
    {
        //mengambil semua slots di ui
        List<GameObject> uiSlots = new List<GameObject>();
        foreach (Transform child in storageUI.transform) { 
            uiSlots.Add(child.gameObject);
        }
        // hapus list item
        selectedStorage.items.Clear();

        List<GameObject>toBeDeleted = new List<GameObject>();
        // ambil inventory item dan konvert ke string'

        foreach (GameObject slot in uiSlots) {
            if (slot.transform.childCount > 0) { 
                //hapus clone
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");

                selectedStorage.items.Add(result);
                toBeDeleted.Add(slot.transform.GetChild(0).gameObject);
            }
        }
        foreach (GameObject obj in toBeDeleted) {
            Destroy(obj);
        
        }
    }

    public void SetSelectedStorage(StorageBox storage)
    {
        selectedStorage = storage;
    }

    private GameObject GetRelevantUI(StorageBox storage)
    {
        // Create a switch for other types
        return storageBoxSmallUI;
    }
}
