using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBox : MonoBehaviour
{
    public bool playerInRange;
    [SerializeField] public List<string> items ;

    public enum BoxType { 
        smallBox,
        BigBox
    }
    public BoxType tipeBox;

    private void Update()
    {
        float jarak = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (jarak < 10f)
        {
            playerInRange = true;
        }
        else { 
            playerInRange= false;
        }
    }
}
