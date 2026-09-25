using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestRow : MonoBehaviour
{
    public TextMeshProUGUI questName;
    public TextMeshProUGUI questGiver;

    public Button trackingBtn;

    public bool isActive;
    public bool isTracking;

    public Text JumlahKoin;

    public Image HadiahPertama;
    public Text jumlahHadiahPertama;

    public Image HadiahKedua;
    public Text jumlahHadiahKedua;

    public Quest thisQuest;

    public void Start()
    {
        trackingBtn.onClick.AddListener(() => {
            if (isActive) {
                if (isTracking)
                {
                    isTracking = false;
                    trackingBtn.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Tidak melacak";
                    QuestManager.Instance.batalkanPelacakan(thisQuest);
                }
                else
                {
                    isTracking = true;
                    trackingBtn.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "melacak";
                    QuestManager.Instance.LacakQuest(thisQuest);
                }
            }
        });
    }
}
