using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] playerStats; //kesehatan, kalori dan hidrasi 
    public float[] playerPosition; //posisi x,y,z dan rotasi 
    public string[] inventory;
    public string[] quickSlot;

    //public string[] inventory;
    public PlayerData(float[] _playerStats, float[] _playerPos, string[] _inventory, string[] _quickSlot)
    {
        playerStats = _playerStats;
        playerPosition = _playerPos;
        inventory = _inventory;
        quickSlot = _quickSlot;
    }


}
