using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI buttonText;

    public int slotNumber;

    public GameObject alertUI;
    Button yesBTN;
    Button noBTN;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        yesBTN = alertUI.transform.Find("YesButton").GetComponent<Button>();
        noBTN = alertUI.transform.Find("NoButton").GetComponent<Button>();
    }

    public void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (SaveManager.Instance.isSlotEmpty(slotNumber))
            {
                SaveConfirmed();
            }
            else
            {
                DisplayOverride();

            }
        });
    }

    private void Update()
    {
        if (SaveManager.Instance.isSlotEmpty(slotNumber))
        {
            buttonText.text = "Empty";

        }
        else {
            buttonText.text = PlayerPrefs.GetString("Slot" + slotNumber + "Description");
        
        }
    }

    public void DisplayOverride() { 
        alertUI.SetActive(true);
        yesBTN.onClick.AddListener(() => {
            SaveConfirmed();
            alertUI.SetActive(false);
        
        });
        noBTN.onClick.AddListener(() => {
            alertUI.SetActive(false);
        });
    
    }

    private void SaveConfirmed() {
        SaveManager.Instance.SaveGame(slotNumber);

        DateTime dt = DateTime.Now;
        string time = dt.ToString("yyyy-MM-dd HH:mm");

        string Description = "Saved Game" + slotNumber + " | " + time;

        buttonText.text = Description;
        PlayerPrefs.SetString("Slot" + slotNumber + "Description", Description);

        SaveManager.Instance.DeselectButton();

    }


}
