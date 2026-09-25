using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; set; }

    public GameObject menuCanvas;
    public GameObject UiCanvas;
    public GameObject saveMenu;
    public GameObject settingMenu;
    public GameObject Menu;

    public bool isMenuOpen;

    public int CurrentFront = 0;

    public int setAsFront() {
        return CurrentFront++;
    
    }
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !isMenuOpen)
        {
            UiCanvas.SetActive(false);
            menuCanvas.SetActive(true);

            isMenuOpen = true;
            movementManager.instance.aktifkanBergerak(false);
            movementManager.instance.aktifkaanMelihat(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SelectionManager.instance.DisableSelection();
            SelectionManager.instance.GetComponent<SelectionManager>().enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.M) && isMenuOpen)
        {
            saveMenu.SetActive(false);
            settingMenu.SetActive(false);
            Menu.SetActive(true);

            UiCanvas.SetActive(true);
            menuCanvas.SetActive(false);

            isMenuOpen = false;
            movementManager.instance.aktifkanBergerak(true);
            movementManager.instance.aktifkaanMelihat(true);

            if (CraftingSystem.instance.isOpen == false && InventorySystem.Instance.isOpen == false) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

            }
            SelectionManager.instance.EnableSelection();
            SelectionManager.instance.GetComponent<SelectionManager>().enabled = true;
        }
    }


}
