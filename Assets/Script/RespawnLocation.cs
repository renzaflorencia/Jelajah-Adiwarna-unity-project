using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnLocation : MonoBehaviour
{
    public bool isRegistered = false;
    public GameObject indicator;


    private void Start()
    {
        PlayerState.Instance.onRespawnRegistered += UnRegisterLocation;
    }


    private void RegisterLocation() {
        PlayerState.Instance.SetRegisteredLocation(this);
        isRegistered = true;
        indicator.SetActive(true);
    }

    private void UnRegisterLocation() {
        if (PlayerState.Instance.registerRespawnLocation != this) {
            isRegistered = false;
            indicator.SetActive(false);
        
        }
    }
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            if (!isRegistered) { 
                RegisterLocation();
            }
        }

    }
}
