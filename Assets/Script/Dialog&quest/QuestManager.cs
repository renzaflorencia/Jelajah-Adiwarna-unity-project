using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; set; }

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

    public List<Quest> SemuaQuestAktif;
    public List<Quest> semuaQuestSelesai;

    [Header("QuestMenu")]
    public GameObject QuestMenu;
    public bool BukaQuestMenu;

    public GameObject PrefabQuestAktif;
    public GameObject PrefabQuestSelesai;

    public GameObject KontenQuestMenu;

    [Header("Pelacak Quest")]
    public GameObject PelacakanKontenQuest;
    public GameObject trackerRowPrefab;

    public List<Quest> semuaPelacakanQuest;

    public void LacakQuest(Quest quest) { 
        semuaPelacakanQuest.Add(quest);
        refreshTrackerList();
    }

    public void batalkanPelacakan(Quest quest) {
        semuaPelacakanQuest.Remove(quest);
        refreshTrackerList();
    }

    public void refreshTrackerList()
    {
        foreach (Transform child in PelacakanKontenQuest.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Quest lacakQuest in semuaPelacakanQuest) {
            GameObject lacakPrefab = Instantiate(trackerRowPrefab, Vector3.zero, Quaternion.identity);
            lacakPrefab.transform.SetParent(PelacakanKontenQuest.transform, false);

            TrackerRow tRow = lacakPrefab.GetComponent<TrackerRow>();

            tRow.judul.text = lacakQuest.questName; //quest.cs
            tRow.deskripsi.text = lacakQuest.questDescription;

            var reg1 = lacakQuest.info.PersyaratanItemPertama;
            var reg1Amount = lacakQuest.info.JumlahPersyaratanPertama;
            var reg2 = lacakQuest.info.PersyaratanItemKedua;
            var reg2Amount = lacakQuest.info.JumlahPersyaratanKedua;


            if (reg2 != "") {
                tRow.persyaratan.text = $"{reg1}" + InventorySystem.Instance.CheckJumlahItem(reg1)  + "/" + $"{reg1Amount}\n" + //jika punya 2 persyaratan
                    $"{reg2}" + InventorySystem.Instance.CheckJumlahItem(reg2) + "/" + $"{reg2Amount}\n";
            }
            else {
                tRow.persyaratan.text = $"{reg1}" + InventorySystem.Instance.CheckJumlahItem(reg1) + "/" + $"{reg1Amount}\n";
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !BukaQuestMenu)
        {
            QuestMenu.SetActive(true);
            //posisi menu UI
            QuestMenu.GetComponentInChildren<Canvas>().sortingOrder = MenuManager.Instance.setAsFront();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.instance.DisableSelection();
            SelectionManager.instance.GetComponent<SelectionManager>().enabled = false;
            BukaQuestMenu = true;

        }
        else if (Input.GetKeyDown(KeyCode.Q) && BukaQuestMenu)
        {
            QuestMenu.SetActive(false);

            if (!CraftingSystem.instance.isOpen || !InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.instance.EnableSelection();
                SelectionManager.instance.GetComponent<SelectionManager>().enabled = true;
            }

            BukaQuestMenu = false;
        }
    }

    public void TambahkanQuestAktif(Quest quest) { 
        SemuaQuestAktif.Add(quest);
        LacakQuest(quest);
        RefreshQuestList();
    }

    public void tandaQuestSelesai(Quest quest) { 
        //hapus quest dari list akrif 
        SemuaQuestAktif.Remove(quest);
        //menambahkan ke list selesai
        semuaQuestSelesai.Add(quest);
        batalkanPelacakan(quest);
        RefreshQuestList();
    }

    public void RefreshQuestList()
    {
        foreach (Transform child in KontenQuestMenu.transform) {
            Destroy(child.gameObject);
        }

        foreach (Quest activeQuest in SemuaQuestAktif)
        {
            GameObject questPrefab = Instantiate(PrefabQuestAktif, Vector3.zero, Quaternion.identity);
            questPrefab.transform.SetParent(KontenQuestMenu.transform, false);

            QuestRow qRow = questPrefab.GetComponent<QuestRow>();

            qRow.thisQuest = activeQuest;
            qRow.questName.text = activeQuest.questName;
            qRow.questGiver.text = activeQuest.questGiver;

            qRow.isActive = true;
            qRow.isTracking = true;

            qRow.JumlahKoin.text = $"{activeQuest.info.HadiahKoin}";
            //qRow.HadiahPertama.sprite = "";
            qRow.jumlahHadiahPertama.text = "";

            //qRow.HadiahKedua.sprite = "";
            qRow.jumlahHadiahKedua.text = "";
        }

        foreach (Quest completedQuest in semuaQuestSelesai)
        {
            GameObject questPrefab = Instantiate(PrefabQuestSelesai, Vector3.zero, Quaternion.identity);
            questPrefab.transform.SetParent(KontenQuestMenu.transform, false);

            QuestRow qRow = questPrefab.GetComponent<QuestRow>();

            qRow.questName.text = completedQuest.questName;
            qRow.questGiver.text = completedQuest.questGiver;

            qRow.isActive = false;
            qRow.isTracking = false;

            qRow.JumlahKoin.text = $"{completedQuest.info.HadiahKoin}";
            //qRow.HadiahPertama.sprite = "";
            qRow.jumlahHadiahPertama.text = "";

            //qRow.HadiahKedua.sprite = "";
            qRow.jumlahHadiahKedua.text = "";
        }

    }
}
