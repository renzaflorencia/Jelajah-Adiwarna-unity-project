using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName ="ScriptableObjects/QuestInfo", order = 1)]
public class QuestInfo : ScriptableObject
{
    [TextArea(5,10)]
    public List<string> initialDialog;

    [Header("opsi")]
    [TextArea(5, 10)]
    public string TerimaOpsi; //accept option
    [TextArea(5, 10)]
    public string TerimaJawaban; //accept answer

    [TextArea(5, 10)]
    public string OpsiTolak; //decline option
    [TextArea(5, 10)]
    public string OpsiJawaban; //decline answwer

    [TextArea(5, 10)]
    public string KembaliSetelahDiTolak; //comebackafterdecline
    [TextArea(5, 10)]
    public string ProsesKembali; //comebackinproses

    [TextArea(5, 10)]
    public string KembaliSelesai; //comeBackCompleted
    [TextArea(5, 10)]
    public string Akhirkata; //finalwords

    [Header("Hadiah")]
    public int HadiahKoin; //coinreward
    public string HadiahItem1; //rewarditem1
    public string HadiahItem2; //rewarditem2

    [Header("Persyaratan")]
    public string PersyaratanItemPertama;
    public int JumlahPersyaratanPertama;

    public string PersyaratanItemKedua;
    public int JumlahPersyaratanKedua;
}