using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnviromentData 
{
    public List<string> pickUpItem;
    public List<TreeData> treeData;
    public List<string> animals;
    public List<StorageData> storage;
    public EnviromentData(List<string> _pickUpItem, List<TreeData> _treeData, List<string> _animals , List<StorageData> _storage) {
        pickUpItem = _pickUpItem;
        treeData = _treeData;
        animals = _animals;
        storage = _storage;

    } 
}
[System.Serializable]
public class StorageData
{
    public List <string> items;
    public Vector3 position;
    public Vector3 rotation;
}
[System.Serializable]
public class TreeData { 
    public string name;
    public Vector3 position;
    public Vector3 rotation;

}
