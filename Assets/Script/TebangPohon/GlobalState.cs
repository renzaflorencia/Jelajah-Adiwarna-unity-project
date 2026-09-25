using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GlobalState : MonoBehaviour
{
    public static GlobalState instance { get; set; }

    public float resourceHealth;
    public float resourceHealthMax;

    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    
}
