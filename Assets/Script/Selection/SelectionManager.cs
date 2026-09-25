using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance { get; set; }
    public bool onTarget;
    public GameObject selectedObject;
    public GameObject interaction_Info_UI;
    Text interaction_text;

    public Image centerDotImage;
    public Image handIcon;

    public bool handIsVisible;

    //pohon 
    public GameObject selectedTree;
    public GameObject chopHolder;
    //chest
    public GameObject selectionStorageBox;
    //campfire
    public GameObject selectionCampfire;

    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;
            InteractableObject Interactable = selectionTransform.GetComponent<InteractableObject>();

            TebangPohon tebangPohon = selectionTransform.GetComponent<TebangPohon>();

            //sistem belanja 
            penjual belanja = selectionTransform.GetComponent<penjual>();

            if (belanja && belanja.playerinRange) {

                if (belanja.berbicaraDenganPlayer == false)
                {
                    interaction_text.text = "Talk";
                    interaction_Info_UI.SetActive(true);

                }
                else {
                    interaction_text.text = "";
                    interaction_Info_UI.SetActive(false);

                }
                if (Input.GetMouseButtonDown(0) && belanja.berbicaraDenganPlayer == false)
                {
                    belanja.berbicara();

                }
            }



            NPC npc = selectionTransform.GetComponent<NPC>();

            //----npc----//
            if (npc && npc.playerInRange)
            {
                interaction_text.text = "Talk";
                interaction_Info_UI.SetActive(true);

                if (Input.GetMouseButtonDown(0) && npc.TalkingWithNPC == false) {
                    npc.mulaiPercakapan();
                }

                if (DialogSystem.Instance.dialogUIActive) {
                    interaction_Info_UI.SetActive(false);
                    centerDotImage.gameObject.SetActive(false);
                
                };

            }

            //----tebang pohon----
            if (tebangPohon && tebangPohon.playerInRange)
            {
                tebangPohon.bisaTebang = true;
                selectedTree = tebangPohon.gameObject;
                chopHolder.gameObject.SetActive(true);
            }
            else
            {
                if (selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<TebangPohon>().bisaTebang = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }
            }

            //---interaksi---

            if (Interactable && Interactable.playerInRange)
            {
                onTarget = true;
                selectedObject = Interactable.gameObject;
                interaction_text.text = Interactable.GetItemName();
                interaction_Info_UI.SetActive(true);

                centerDotImage.gameObject.SetActive(false);
                handIcon.gameObject.SetActive(true);

                handIsVisible = true;   
  
            }
            
            //--- box ---

            StorageBox storageBox = selectionTransform.GetComponent<StorageBox>();
            if (storageBox && storageBox.playerInRange && PlacementSystem.Instance.inPlacementMode == false) {
                interaction_text.text = "Open";
                interaction_Info_UI.SetActive(true);

                selectionStorageBox = storageBox.gameObject;

                if (Input.GetMouseButtonDown(0)) {
                    StorageManager.Instance.OpenBox(storageBox);
                }
            }
            else
            {
                if (selectionStorageBox != null) {
                    selectionStorageBox = null;
                }
            }

            //campfire

            Campfire campfire = selectionTransform.GetComponent<Campfire>();
            if (campfire && campfire.playerInRange && PlacementSystem.Instance.inPlacementMode == false)
            {
                interaction_text.text = "Interaksi";
                interaction_Info_UI.SetActive(true);

                selectionCampfire = campfire.gameObject;

                if (Input.GetMouseButtonDown(0) && campfire.isCooking == false)
                {
                    campfire.OpenUI();
                }
            }
            else
            {
                if (selectionCampfire != null)
                {
                    selectionCampfire = null;
                }
            }

            //-----hit hewan----

            animal hewan = selectionTransform.GetComponent<animal>();

            if (hewan && hewan.playerInRange) {
                if (hewan.isDead) {
                    interaction_text.text = "Loot";
                    interaction_Info_UI.SetActive(true);

                    centerDotImage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);
                    handIsVisible = true;
                    if (Input.GetMouseButtonDown(0)) {
                        Lootable lootable = hewan.GetComponent<Lootable>();
                        Loot(lootable);
                    }
                }
                else {
                    interaction_text.text = hewan.animalName;
                    interaction_Info_UI.SetActive(true);
                    centerDotImage.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);
                    handIsVisible = false;

                    if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.memegangSenjata() && EquipSystem.Instance.IsThereASwingLock() == false)
                    {
                        StartCoroutine(memberikanDmgKe(hewan, 0.3f, EquipSystem.Instance.weaponDamage()));
                    }
                }  
            }
            if (!Interactable && !hewan) {
                onTarget = false;
                handIsVisible = false;

                centerDotImage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
            }
            if (!npc && !Interactable && !hewan && !tebangPohon && !storageBox && !campfire && !belanja) {
                interaction_text.text = "";
                interaction_Info_UI.SetActive(false);
            
            }
        }
    }

    private void Loot(Lootable lootable)
    {
        if (lootable.LootCalculated == false) { 
            List<LootRecieved> recievedLoot = new List<LootRecieved>();
            foreach (LootPossibility loot in lootable.possibleLoot) {

                var jumlahLoot = UnityEngine.Random.Range(loot.jumlahMin, loot.jumlahMax + 1);
                if (jumlahLoot > 0) { 
                    LootRecieved lt = new LootRecieved();
                    lt.item = loot.item;
                    lt.jumlah= jumlahLoot;

                    recievedLoot.Add(lt);
                }
            }
            lootable.finalLoot = recievedLoot;
            lootable.LootCalculated = true;
        }

        //memunculkan hasil rampasan 
        Vector3 PosisiLootSpown = lootable.gameObject.transform.position;

        foreach (LootRecieved lootRecieved in lootable.finalLoot) {
            for (int i = 0; i < lootRecieved.jumlah; i++) {
                GameObject lootSpown = Instantiate(Resources.Load<GameObject>(lootRecieved.item.name + "_Model"),
                   new Vector3(PosisiLootSpown.x, PosisiLootSpown.y, PosisiLootSpown.z),
                   Quaternion.Euler(0, 0, 0));
            }
        }
        //menghilangkan tubuh jarahan 
        Destroy(lootable.gameObject);
    }

    IEnumerator memberikanDmgKe(animal hewan, float delay, int damage)
    {
        yield return new WaitForSeconds(delay); 

        hewan.TakeDamage(damage);
    }

    public void DisableSelection() {
        handIcon.enabled = false;
        centerDotImage.enabled = false;
        interaction_Info_UI.SetActive(false);
        selectedObject = null;
    }

    public void EnableSelection()
    {
        handIcon.enabled = false;
        centerDotImage.enabled = true;
        interaction_Info_UI.SetActive(true);
    }
}
