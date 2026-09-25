using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI, SurvivalScreenUI, RefineScreenUI ;

    public List<string> inventoryItemList = new List<string>();

    //tombol Kategori 
    Button toolsBTN, survivalBTN, RefineBTN;

    //tombol craft
    Button craftKerisBTN, craftPapanBTN,craftStorageBoxBTN;

    Text KerisReg1, KerisReg2, papanReg1,petiReg1,petiReg2;

    public bool isOpen;

    public Blueprint KerisBLP = new Blueprint("Keris",1, 2, "Stone", 2, "Stick",3);
    public Blueprint PapanBLP = new Blueprint("Papan",2, 1, "Kayu", 1, "", 0);
    public Blueprint PetiBLP = new Blueprint("peti", 1, 2, "Kayu", 2, "Stone", 2);
    public static CraftingSystem instance { get; set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else { 
            instance = this;
        } 
    }


    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        //crafting screen
        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

        survivalBTN = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalBTN.onClick.AddListener(delegate { OpenSurvivalCategory(); });

        RefineBTN = craftingScreenUI.transform.Find("RefineButton").GetComponent<Button>();
        RefineBTN.onClick.AddListener(delegate { OpenRefineCategory(); });

        //keris
        KerisReg1 = toolsScreenUI.transform.Find("Keris").transform.Find("Reg1").GetComponent<Text>();
        KerisReg2 = toolsScreenUI.transform.Find("Keris").transform.Find("Reg2").GetComponent<Text>();

        craftKerisBTN = toolsScreenUI.transform.Find("Keris").transform.Find("Button").GetComponent<Button>();
        craftKerisBTN.onClick.AddListener(delegate { CraftAnyItem(KerisBLP);});

        //papan
        papanReg1 = RefineScreenUI.transform.Find("papan").transform.Find("Reg1").GetComponent<Text>();

        craftPapanBTN = RefineScreenUI.transform.Find("papan").transform.Find("Button").GetComponent<Button>();
        craftPapanBTN.onClick.AddListener(delegate { CraftAnyItem(PapanBLP); });

        //peti
        petiReg1 = SurvivalScreenUI.transform.Find("Peti").transform.Find("Reg1").GetComponent<Text>();
        petiReg2 = SurvivalScreenUI.transform.Find("Peti").transform.Find("Reg2").GetComponent<Text>();

        craftStorageBoxBTN = SurvivalScreenUI.transform.Find("Peti").transform.Find("Button").GetComponent<Button>();
        craftStorageBoxBTN.onClick.AddListener(delegate { CraftAnyItem(PetiBLP); });
    }

    void OpenToolsCategory() {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
        SurvivalScreenUI.SetActive(false);
        RefineScreenUI.SetActive(false);
        RefreshNeededItems();
    }

    void OpenSurvivalCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        SurvivalScreenUI.SetActive(true);
        RefineScreenUI.SetActive(false);
        RefreshNeededItems();
    }

    void OpenRefineCategory()
    {
        craftingScreenUI.SetActive(false);
        RefineScreenUI.SetActive(true);
        toolsScreenUI.SetActive(false);
        SurvivalScreenUI.SetActive(false);
        RefreshNeededItems();
    }


    void CraftAnyItem(Blueprint blueprintToCraft) {

        //sound effect
        SoundSystem.instance.PlaySound(SoundSystem.instance.craftingSound);

        StartCoroutine(CraftedDelayForSound(blueprintToCraft));
     

        if (blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Reg1, blueprintToCraft.Reg1amount);

        }
        else if (blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Reg1, blueprintToCraft.Reg1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Reg2, blueprintToCraft.Reg2amount);
        }

        StartCoroutine(calculate());
    }


    public IEnumerator calculate() {
        yield return 0;
        InventorySystem.Instance.ReCalculateList();
        RefreshNeededItems();
    }

    IEnumerator CraftedDelayForSound(Blueprint blueprintToCraft) {

        yield return new WaitForSeconds(1f);
        //menghasilkan jumlah barang sesuai dengan cetak biru
        for (var i = 0; i < blueprintToCraft.numberOfItems; i++)
        {
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName, true); //true
        }

    }

    // Update is called once per frame
    void Update()
    {

        //RefreshNeededItems();

        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {
            craftingScreenUI.SetActive(true);
            craftingScreenUI.GetComponentInParent<Canvas>().sortingOrder = MenuManager.Instance.setAsFront();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.instance.DisableSelection();
            SelectionManager.instance.GetComponent<SelectionManager>().enabled = false;
            isOpen = true;
            movementManager.instance.aktifkanBergerak(false);
            movementManager.instance.aktifkaanMelihat(false);
            RefreshNeededItems() ;
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            SurvivalScreenUI.SetActive(false);
            RefineScreenUI.SetActive(false);

            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.instance.EnableSelection();
                SelectionManager.instance.GetComponent<SelectionManager>().enabled = true;
            }
            isOpen = false;
            movementManager.instance.aktifkanBergerak(true);
            movementManager.instance.aktifkaanMelihat(true);
        }
    }



    public void RefreshNeededItems() {

        int stone_count = 0;
        int stick_count = 0;
        int kayu_count = 0;
        int papan_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList) {
            switch (itemName) {
                case "Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break ;
                case "Kayu":
                    kayu_count += 1;
                    break;
                case "Papan":
                    papan_count+= 1;
                    break ;
            } 
        }
        //keris 
        KerisReg1.text = "2 Stone["+ stone_count + "]";
        KerisReg2.text = "3 Stick[" + stick_count + "]";

        if (stone_count >= 2 && stick_count >= 3 && InventorySystem.Instance.CheckIfFull(1))
        {
            craftKerisBTN.gameObject.SetActive(true);
        }
        else {
            craftKerisBTN.gameObject.SetActive(false);
        }

        //papan
        papanReg1.text = "1 Kayu[" + kayu_count + "]";

        if (kayu_count >= 1 && InventorySystem.Instance.CheckIfFull(2))
        {
            craftPapanBTN.gameObject.SetActive(true);
        }
        else
        {
            craftPapanBTN.gameObject.SetActive(false);
        }

        //peti
        petiReg1.text = "2 Stone["+ stone_count +"]";
        petiReg2.text = "2 Papan[" + papan_count + "]";

        if (stone_count >= 2 && papan_count >= 2 && InventorySystem.Instance.CheckIfFull(1))
        {
            craftStorageBoxBTN.gameObject.SetActive(true);
        }
        else
        {
            craftStorageBoxBTN.gameObject.SetActive(false);
        }
    }
   
}
