using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable : MonoBehaviour
{
    public List<LootPossibility> possibleLoot;
    public List<LootRecieved> finalLoot;

    public bool LootCalculated;
}
[System.Serializable]

public class LootPossibility {
    public GameObject item;
    public int jumlahMin;
    public int jumlahMax;

}

[System.Serializable]
public class LootRecieved {
    public GameObject item;
    public int jumlah;

}