using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertDialogManager : MonoBehaviour
{
    public GameObject dialogBox;
    public Text pesanText;
    public Button OkButton;
    public Button CancelButton;

    private System.Action<bool> responceCallBack;

    private void Start()
    {
        dialogBox.SetActive(false);
        OkButton.onClick.AddListener(() => HandleResponse(true));
        CancelButton.onClick.AddListener(() => HandleResponse(false));
    }

    public void ShowDialog(string message, System.Action<bool> callback) { 
        responceCallBack = callback;
        pesanText.text = message;
        dialogBox.SetActive(true);
    
    }

    private void HandleResponse(bool response)
    {
        dialogBox.SetActive(false);
        responceCallBack?.Invoke(response);
    }
}
