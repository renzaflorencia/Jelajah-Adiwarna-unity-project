using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SaveManager;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance { get; set; }
    public Button backBTN;

    public Slider MasterSlider;
    public GameObject MasterValue;

    public Slider MusicSlider;
    public GameObject MusicValue;

    public Slider EffectSlider;
    public GameObject EffectValue;

    private void Start()
    {
        backBTN.onClick.AddListener(() =>
       {
           SaveManager.Instance.SaveVolumeSettings(MusicSlider.value, EffectSlider.value, MasterSlider.value);
           print("disimpan ke preferensi pemain");
        });

        StartCoroutine(ApplySettings());

    }

    private IEnumerator ApplySettings() {
        SetVolume();
        //grafik setting 
        // key Bindinds
        yield return new WaitForSeconds(0.1f);
    }

    private void SetVolume() { 
        VolumeSettings volumeSettings = SaveManager.Instance.LoadVolumeSettings(); 
        MasterSlider.value = volumeSettings.master;
        MusicSlider.value = volumeSettings.Music;
        EffectSlider.value = volumeSettings.Effect;

        print("Volume setting ditambahkan");
        
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
        MasterValue.GetComponent<TextMeshProUGUI>().text = "" + (MasterSlider.value) + "";
        MusicValue.GetComponent<TextMeshProUGUI>().text = "" + (MusicSlider.value) + "";
        EffectValue.GetComponent<TextMeshProUGUI>().text = "" + (EffectSlider.value) + "";
    }
}
