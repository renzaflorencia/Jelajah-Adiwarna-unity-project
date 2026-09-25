using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class penjual : MonoBehaviour
{
    public bool playerinRange;
    public bool berbicaraDenganPlayer;

    public GameObject UIdialogPenjual;
    public Button buyBTN;
    public Button sellBTN;
    public Button exitBtn;

    public GameObject buyPanelUI;
    public GameObject sellPanelUI;



    public void Start()
    {
        UIdialogPenjual.SetActive(false);
        buyBTN.onClick.AddListener(modeBeli);
        sellBTN.onClick.AddListener(modeJual);
        exitBtn.onClick.AddListener(berhentiBerbicara);
    }

    private void modeJual()
    {
        sellPanelUI.SetActive(true);
        buyPanelUI.SetActive(false);

        HideDialogUI();

    }

    private void modeBeli()
    {
        sellPanelUI.SetActive(false);
        buyPanelUI.SetActive(true);

        HideDialogUI();
    }

    public void modeDialog()
    {
        DisplayDialogUI();
        sellPanelUI.SetActive(false);
        buyPanelUI.SetActive(false);
        
    }

    public void berbicara() {
        berbicaraDenganPlayer = true;
        DisplayDialogUI();

        movementManager.instance.aktifkanBergerak(false);
        movementManager.instance.aktifkaanMelihat(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void berhentiBerbicara()
    {
        berbicaraDenganPlayer = false;
        HideDialogUI();

        movementManager.instance.aktifkanBergerak(true);
        movementManager.instance.aktifkaanMelihat(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void DisplayDialogUI()
    {
        UIdialogPenjual.SetActive(true);
    }

    private void HideDialogUI()
    {
        UIdialogPenjual.SetActive(false);
    }

    #region || ----trigger----||
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){ 
            playerinRange = true;
        
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player")){
            playerinRange = false;

        }
    }

    #endregion
}
