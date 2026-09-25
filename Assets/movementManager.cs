using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementManager : MonoBehaviour
{
    public static movementManager instance { get; set; }

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

    public bool bisaBergerak = true;//keyboard move
    public bool bisaMelihatSekitar = true;//mouse move

    public void aktifkanBergerak(bool trigger)
    {
        bisaBergerak = trigger;

    }

    public void aktifkaanMelihat(bool trigger)
    {
        bisaMelihatSekitar = trigger;
    }

}
