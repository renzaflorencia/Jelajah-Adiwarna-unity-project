using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CookingData", menuName = "ScriptableObjects/CookingData", order = 1)]
public class CookingData : ScriptableObject
{
    public List<string> BahanBakarValid = new List<string>();
    public List<CookableFood> makananValid = new List<CookableFood>();

}

[System.Serializable]
public class CookableFood{
    public string name;
    public float waktuUntukMemasak;
    public string namaMasakan; // cooked food name
    
}
