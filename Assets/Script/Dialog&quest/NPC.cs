using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public bool playerInRange;
    public bool TalkingWithNPC;

    TextMeshProUGUI NpcDialog;

    Button OptionBtn1;
    TextMeshProUGUI OptionButton1Text;

    Button OptionBtn2;
    TextMeshProUGUI OptionButton2Text;

    public List<Quest> quests;
    public Quest QuestAktifSekarang = null; //currentActiveQuest
    public int QuestIndeks = 0; //activequestIndeks
    public bool interaksiPertama = true; //firstTimeInteraction
    public int dialogSekarang; //currentDialog

    private void Start()
    {
        NpcDialog = DialogSystem.Instance.dialogText;

        OptionBtn1 = DialogSystem.Instance.Option1BTN;
        OptionButton1Text = DialogSystem.Instance.Option1BTN.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        OptionBtn2 = DialogSystem.Instance.Option2BTN;
        OptionButton2Text = DialogSystem.Instance.Option2BTN.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
    }
    public void mulaiPercakapan()
    {
        TalkingWithNPC = true;
        MelihatPlayer();

        //interaksi pertama dengan npc
        if (interaksiPertama) {
            interaksiPertama = false;
            QuestAktifSekarang = quests[QuestIndeks];
            mulaiQuest(); //startquestinitialdialog
            dialogSekarang = 0;
        }
        else { //interaksi dengan npc setelah pertama kali
               
            //jika kembali setelah menolak quest
            if (QuestAktifSekarang.declined) {
                DialogSystem.Instance.OpenDialog();
                NpcDialog.text = QuestAktifSekarang.info.KembaliSetelahDiTolak;

                terimaDanTolakOpsi();
            }
        }

        //kembali ketika quest masih dalam proses
        if (QuestAktifSekarang.accepted && QuestAktifSekarang.isCompleted == false) {
            if (PersyaratanQuestSelesai()) {
                SubmitPersyaratanItem();

                DialogSystem.Instance.OpenDialog();
                NpcDialog.text = QuestAktifSekarang.info.KembaliSelesai;

                OptionButton1Text.text = "[ambil hadiah]";
                OptionBtn1.onClick.RemoveAllListeners();
                OptionBtn1.onClick.AddListener(() => {
                    TerimaHadiahDanQuestSelesai();
                });
                OptionBtn2.gameObject.SetActive(false);
            }
            else {
                DialogSystem.Instance.OpenDialog();
                NpcDialog.text = QuestAktifSekarang.info.ProsesKembali;

                OptionButton1Text.text = "[Close]";
                OptionBtn1.onClick.RemoveAllListeners();
                OptionBtn1.onClick.AddListener(() => {
                    DialogSystem.Instance.CloseDialog();
                    TalkingWithNPC = false;
                });
                OptionBtn2.gameObject.SetActive(false);
            }

            if (QuestAktifSekarang.isCompleted == true) {
                DialogSystem.Instance.OpenDialog();
                NpcDialog.text = QuestAktifSekarang.info.Akhirkata;

                OptionButton1Text.text = "[Close]";
                OptionBtn1.onClick.RemoveAllListeners();
                OptionBtn1.onClick.AddListener(() => {
                    DialogSystem.Instance.CloseDialog();
                    TalkingWithNPC = false;
                });
            }

            //jika ada quest lain
            if (QuestAktifSekarang.initialDialogCompleted == false) {
                mulaiQuest();
            
            }
        }
    }

    private void terimaDanTolakOpsi()
    {
        OptionButton1Text.text = QuestAktifSekarang.info.TerimaOpsi;
        OptionBtn1.onClick.RemoveAllListeners();
        OptionBtn1.onClick.AddListener(() => {
            questDiterima();
        });

        OptionBtn2.gameObject.SetActive(true);
        OptionButton2Text.text = QuestAktifSekarang.info.OpsiTolak;
        OptionBtn2.onClick.RemoveAllListeners();
        OptionBtn2.onClick.AddListener(() => {
            QuestDitolak();
        });
    }

    private void SubmitPersyaratanItem()
    {
        string persyaratanItemPertama = QuestAktifSekarang.info.PersyaratanItemPertama;
        int jumlahPersyaratanPertama = QuestAktifSekarang.info.JumlahPersyaratanPertama;

        if (persyaratanItemPertama != "") {
            InventorySystem.Instance.RemoveItem(persyaratanItemPertama, jumlahPersyaratanPertama);
        }

        string persyaratanItemkedua = QuestAktifSekarang.info.PersyaratanItemKedua;
        int jumlahPersyaratankedua = QuestAktifSekarang.info.JumlahPersyaratanKedua;

        if (persyaratanItemkedua != "")
        {
            InventorySystem.Instance.RemoveItem(persyaratanItemkedua, jumlahPersyaratankedua);
        }
    }

    private bool PersyaratanQuestSelesai()
    {
        print("mengecek persyaratan");

        //persyaratan item pertama
        string persyaratanItemPertama = QuestAktifSekarang.info.PersyaratanItemPertama;
        int jumlahPersyaratanPertama = QuestAktifSekarang.info.JumlahPersyaratanPertama;

        var firstItemCounter = 0;

        foreach (string item in InventorySystem.Instance.itemList) {
            if (item == persyaratanItemPertama) { 
                firstItemCounter++;
            
            }
        }

        //persyaratan item kedua

        String persyaratanItemKedua = QuestAktifSekarang.info.PersyaratanItemKedua;
        int jumlahPersyaratanKedua = QuestAktifSekarang.info.JumlahPersyaratanKedua;

        var secondItemCounter = 0;
        foreach (String item in InventorySystem.Instance.itemList) {
            if (item == persyaratanItemKedua) { 
                secondItemCounter++;
            }
        }

        if (firstItemCounter >= jumlahPersyaratanPertama && secondItemCounter >= jumlahPersyaratanKedua)
        {
            return true;
        }
        else { 
            return false;
        }
    }

    private void mulaiQuest() { //startquestinitialdialog
        DialogSystem.Instance.OpenDialog();

        NpcDialog.text = QuestAktifSekarang.info.initialDialog[dialogSekarang];
        OptionButton1Text.text = "Next";
        OptionBtn1.onClick.RemoveAllListeners();
        OptionBtn1.onClick.AddListener(() => { 
            dialogSekarang++;
            CheckDialogSelesai(); //checkIfDialogDone
        });
        OptionBtn2.gameObject.SetActive(false);
    }

    private void CheckDialogSelesai() {
        if (dialogSekarang == QuestAktifSekarang.info.initialDialog.Count - 1) {
            NpcDialog.text = QuestAktifSekarang.info.initialDialog[dialogSekarang];

            QuestAktifSekarang.initialDialogCompleted = true;

            terimaDanTolakOpsi();

        }
        else {
            NpcDialog.text = QuestAktifSekarang.info.initialDialog[dialogSekarang];
            OptionButton1Text.text = "Next";
            OptionBtn1.onClick.RemoveAllListeners();
            OptionBtn1.onClick.AddListener(() => {
                dialogSekarang++;
                CheckDialogSelesai(); //checkIfDialogDone
            });

        }
    }

    private void questDiterima() {
        QuestManager.Instance.TambahkanQuestAktif(QuestAktifSekarang);

        QuestAktifSekarang.accepted = true;
        QuestAktifSekarang.declined = true;

        if (QuestAktifSekarang.NoRequirements) {
            NpcDialog.text = QuestAktifSekarang.info.KembaliSelesai;
            OptionButton1Text.text = "[ambil hadiah]";
            OptionBtn1.onClick.RemoveAllListeners();
            OptionBtn1.onClick.AddListener(() => {
                TerimaHadiahDanQuestSelesai();
            });
            OptionBtn2.gameObject.SetActive(false);
        }
        else {
            NpcDialog.text = QuestAktifSekarang.info.TerimaJawaban;
            CloseDialogUI();
        }
    }

    private void CloseDialogUI()
    {
        OptionButton1Text.text = "[Close]";
        OptionBtn1.onClick.RemoveAllListeners();
        OptionBtn1.onClick.AddListener(() => {
            DialogSystem.Instance.CloseDialog();
            TalkingWithNPC = false;
        });
        OptionBtn2.gameObject.SetActive(false);
    }

    private void TerimaHadiahDanQuestSelesai()
    {

        QuestManager.Instance.tandaQuestSelesai(QuestAktifSekarang);
        QuestAktifSekarang.isCompleted = true;

        var terimaKoin = QuestAktifSekarang.info.HadiahKoin;
        print("kamu menerima " + terimaKoin + " koin emas");

        if (QuestAktifSekarang.info.HadiahItem1 != "") {
            InventorySystem.Instance.AddToInventory(QuestAktifSekarang.info.HadiahItem1, true); //true
        }
        if (QuestAktifSekarang.info.HadiahItem2 != "")
        {
            InventorySystem.Instance.AddToInventory(QuestAktifSekarang.info.HadiahItem2, true); //true
        }
        QuestIndeks++;

        //quest berikutnya
        if (QuestIndeks < quests.Count)
        {
            QuestAktifSekarang = quests[QuestIndeks];
            dialogSekarang = 0;
            DialogSystem.Instance.CloseDialog();
            TalkingWithNPC = false;

        }
        else {
            DialogSystem.Instance.CloseDialog();
            TalkingWithNPC = false;
            print("tidak ada quest");
        }
    }

    private void QuestDitolak() {
        QuestAktifSekarang.declined = true;

        NpcDialog.text = QuestAktifSekarang.info.OpsiJawaban;
        CloseDialogUI();
    }

    public void MelihatPlayer (){
        var player = PlayerState.Instance.playerBody.transform;
        Vector3 direction = player.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
