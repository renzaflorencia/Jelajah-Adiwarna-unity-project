using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }  
        DontDestroyOnLoad(gameObject);

    }

    //json project save
    string JsonPathProject;
    //json external
    string jsonPathPersistant; 
    //binary save path 
    string binaryPath;

    string fileName = "SaveGame";

    public bool isSavingJson;
    public bool isLoading;
    public Canvas loadingScreen;

    private void Start()
    {
        JsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar ;
        jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
        binaryPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    }

    #region ||----- General section ------|| 


    #region ||----- Saving ------|| 

    public void SaveGame(int slotNumber) { 
        allGameData data = new allGameData();
        data.playerData = GetPlayerData();
        data.enviromentData = GetEnviromentData();
        SavingTypeSwitch(data, slotNumber);
    }

    private EnviromentData GetEnviromentData() {
        List<string> itemPickUp = InventorySystem.Instance.itemPickUp;

        // data pohon 
        List<TreeData> treesToSave = new List<TreeData>();

        foreach (Transform tree in EnviromentManager.Instance.allTrees.transform) {
            if (tree.CompareTag("Tree"))
            {
                var td = new TreeData();
                td.name = "Tree_Parent";
                td.position = tree.position;
                td.rotation = new Vector3(tree.rotation.x, tree.rotation.y, tree.rotation.z);

                treesToSave.Add(td);
            }
            else {
                var td = new TreeData();
                td.name = "Stump";
                td.position = tree.position;
                td.rotation = new Vector3(tree.rotation.x, tree.rotation.y, tree.rotation.z);

                treesToSave.Add(td);
            }
            
        }

        //get all animals
        List <string> allAnimals = new List<string>();
        foreach (Transform animalType in EnviromentManager.Instance.allAnimals.transform) {
            foreach (Transform animal in animalType.transform) {
                allAnimals.Add(animal.gameObject.name);
            }
        
        }

        //mendapatkan informasi tentang storage box di scene 
        List<StorageData> allStorage = new List<StorageData>();
        foreach (Transform placeable in EnviromentManager.Instance.Placeables.transform) {
            if (placeable.gameObject.GetComponent<StorageBox>()) { 
                var sd = new StorageData();
                sd.items = placeable.gameObject.GetComponent<StorageBox>().items;
                sd.position = placeable.position;
                sd.rotation = new Vector3(placeable.rotation.x, placeable.rotation.y, placeable.rotation.z);

                allStorage.Add(sd);
            }
        }

        return new EnviromentData(itemPickUp, treesToSave, allAnimals, allStorage);
    
    
    }

    private PlayerData GetPlayerData() {
        float[] playerStats = new float[3];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentCalories;
        playerStats[2] = PlayerState.Instance.currentHydration;

        float[]playerPos = new float[6];
        playerPos[0] = PlayerState.Instance.playerBody.transform.position.x;
        playerPos[1] = PlayerState.Instance.playerBody.transform.position.y;
        playerPos[2] = PlayerState.Instance.playerBody.transform.position.z;

        playerPos[3] = PlayerState.Instance.playerBody.transform.rotation.x;
        playerPos[4] = PlayerState.Instance.playerBody.transform.rotation.y;
        playerPos[5] = PlayerState.Instance.playerBody.transform.rotation.z;
        
        string[] inventory = InventorySystem.Instance.itemList.ToArray();
        string[] quickSlot = GetQuickSlot();
        return new PlayerData(playerStats, playerPos, inventory, quickSlot);
    }

    private string[] GetQuickSlot() { 
        List<string> temp = new List<string>();

        foreach (GameObject slot in EquipSystem.Instance.quickSlotsList) {
            if (slot.transform.childCount != 0) { 
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string cleanName = name.Replace(str2, "");
                temp.Add(cleanName);
            }
        }
        return temp.ToArray();
    }


    public void SavingTypeSwitch(allGameData gameData, int slotNumber) {
        if (isSavingJson)
        {
            JsonSaveGameData(gameData, slotNumber); //json
        }
        else {
            SaveGameData(gameData, slotNumber); //binary
        }
    }


    #endregion

    #region ||----- loading ------|| 
    public allGameData LoadingTypeSwitch(int slotNumber) {
        if (isSavingJson)
        {
            allGameData gameData = JsonLoadGameData(slotNumber);
            return gameData;
        }
        else {
            allGameData gameData = LoadGameData(slotNumber);
            return gameData;
        }
    }

    public void LoadGame(int slotNumber) {
        //player data
        setPlayerData(LoadingTypeSwitch(slotNumber).playerData);

        //enviroment data 
        setEnviromentData(LoadingTypeSwitch(slotNumber).enviromentData);
        isLoading = false;

        DisableLoading();

    }

    private void setEnviromentData(EnviromentData enviromentData) {

        //---pick up 
        foreach (Transform itemType in EnviromentManager.Instance.allItems.transform)
        {
            foreach (Transform item in itemType.transform)
            {
                if (enviromentData.pickUpItem.Contains(item.name))
                {
                    Destroy(item.gameObject);
                }
            }
        }
        InventorySystem.Instance.itemPickUp = enviromentData.pickUpItem;

        //----tree---//
        //hapus semua pohon
        foreach (Transform tree in EnviromentManager.Instance.allTrees.transform) { 
            Destroy(tree.gameObject);
        }
        //tambahkan pohon 
        foreach (TreeData Tree in enviromentData.treeData) {
            var treePrefab = Instantiate(Resources.Load<GameObject>(Tree.name),
                new Vector3(Tree.position.x, Tree.position.y, Tree.position.z),
                Quaternion.Euler(Tree.rotation.x, Tree.rotation.y, Tree.rotation.z));
            
            treePrefab.transform.SetParent(EnviromentManager.Instance.allTrees.transform);
        }

        //hapus hewan yang seharusnya tidak ada 
        foreach (Transform animalType in EnviromentManager.Instance.allAnimals.transform)
        {
            foreach (Transform animal in animalType.transform)
            {
                if (enviromentData.animals.Contains(animal.gameObject.name) == false) {
                    Destroy(animal.gameObject);
                }
            }
        }

        // add storage boxes
        foreach (StorageData storage in enviromentData.storage) {
            var storageBoxPrefab = Instantiate(Resources.Load<GameObject>("Peti_Model"),
            new Vector3(storage.position.x, storage.position.y, storage.position.z),
            Quaternion.Euler(storage.rotation.x, storage.rotation.y, storage.rotation.z));

            storageBoxPrefab.GetComponent<StorageBox>().items = storage.items;
            storageBoxPrefab.transform.SetParent(EnviromentManager.Instance.Placeables.transform);
        }
    }

    private void setPlayerData(PlayerData playerdata) {

        //stats
        PlayerState.Instance.currentHealth = playerdata.playerStats[0];
        PlayerState.Instance.currentCalories = playerdata.playerStats[1];
        PlayerState.Instance.currentHydration = playerdata.playerStats[2];

        //position
        Vector3 loadedPosition;
        loadedPosition.x = playerdata.playerPosition[0];
        loadedPosition.y = playerdata.playerPosition[1];
        loadedPosition.z = playerdata.playerPosition[2];

        PlayerState.Instance.playerBody.transform.position = loadedPosition;

        // rotation
        Vector3 loadedRotation;
        loadedRotation.x = playerdata.playerPosition[3];
        loadedRotation.y = playerdata.playerPosition[4];
        loadedRotation.z = playerdata.playerPosition[5];

        PlayerState.Instance.playerBody.transform.rotation = Quaternion.Euler(loadedRotation);

        //inventory setting 
        foreach (string item in playerdata.inventory) { 
            InventorySystem.Instance.AddToInventory(item, true); //true
        }
        foreach (string item in playerdata.quickSlot) {
            //menemukan slot kosong berikutnya 
            GameObject availableSlot = EquipSystem.Instance.FindNextEmptySlot();
            var itemToAdd = Instantiate(Resources.Load<GameObject>(item));
            itemToAdd.transform.SetParent(availableSlot.transform,false);
        }
        

    }

    public void StartLoadedGame(int slotNumber) {
        ActiveLoading();

        isLoading = true;
        SceneManager.LoadScene("GameScene");

        StartCoroutine(Delay(slotNumber));
    }

    private IEnumerator Delay(int slotNumber) { 
        yield return new WaitForSeconds(1f);
        LoadGame(slotNumber);
    
    }

    #endregion

    #endregion

    #region ||----- Binary section ------|| 
    public void SaveGameData(allGameData gameData, int slotNumber) { 
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        print("Data saved to" + binaryPath + fileName + slotNumber + ".bin");
    }


    public allGameData LoadGameData(int slotNumber) //load game data to binary
    {

     
        if (File.Exists(binaryPath + fileName + slotNumber + ".bin"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(binaryPath+ fileName + slotNumber + ".bin", FileMode.Open);

            allGameData data = formatter.Deserialize(stream) as allGameData;
            stream.Close();

            print("Data dimuat dari " + binaryPath + fileName + slotNumber + ".bin");
            return data;
        }
        else { 
            return null;
        
        }

       

    }

    #endregion

    #region ||----- json section ------|| 
    public void JsonSaveGameData(allGameData gameData, int slotNumber)
    {
        string json = JsonUtility.ToJson(gameData);
        //string enkripsi = Encrypt(json);

        using (StreamWriter writer = new StreamWriter(JsonPathProject + fileName + slotNumber + ".json")) { 
            writer.Write(json);
            print("simpan game di json file: " + JsonPathProject + fileName + slotNumber + ".json");
        };
    }
    public allGameData JsonLoadGameData(int slotNumber) //load game data to binary
    {
        using (StreamReader reader = new StreamReader(JsonPathProject + fileName + slotNumber + ".json")) { 
            string json = reader.ReadToEnd();
            //string decrypted = Decrypt(json);
            allGameData data = JsonUtility.FromJson<allGameData>(json);
            return data;  
        }
    }

    #endregion

    #region ||----- volume settings ------|| 
    //---volume musik

    [System.Serializable]
    public class VolumeSettings { 
        public float Music;
        public float Effect;
        public float master;
    }
    public void SaveVolumeSettings(float _music, float _effects, float _master) {
        VolumeSettings volumeSettings = new VolumeSettings() { 
            Music = _music, 
            Effect = _effects,
            master = _master
        };
        PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSettings));
        PlayerPrefs.Save();
    }
    public VolumeSettings LoadVolumeSettings() {
        return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
   
    }
    public float LoadMusicVolume()
    {
        var volumeSettings =  JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
        return volumeSettings.Music;
    }

    #endregion

    #region || ------ enkripsi ----- ||

    public string Encrypt(string jsonString) {
        string keyword = "1234567";
        string result = "";

        for (int i = 0; i < jsonString.Length; i++) {
            result += (char)(jsonString[i] ^ keyword[i % keyword.Length]);
        } 
        return result;

    }
    public string Decrypt(string encryptedString)
    {
        // XOR dekripsi sama seperti enkripsi
        return Encrypt(encryptedString);
    }




    #endregion

    #region || Loading Screen || 
    public void ActiveLoading() {
        loadingScreen.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        
    }

    public void DisableLoading() {
        loadingScreen.gameObject.SetActive(false);

    
    }

    #endregion

    #region||----- Utility -----||

    public bool FileExists(int slotNumber) {
        if (isSavingJson)
        {
            if (System.IO.File.Exists(JsonPathProject + fileName+ slotNumber + ".json"))
            {
                return true;

            }
            else
            {
                return false;

            }
        }
        else {
            if (System.IO.File.Exists(binaryPath + fileName + slotNumber + ".bin"))
            {
                return true;

            }
            else
            {
                return false ;

            }

        }
    
    }

    public bool isSlotEmpty(int SlotNumber)
    {
        if (FileExists(SlotNumber))
        {
            return false;

        }
        else
        {
            return true;
        }
    }

    public void DeselectButton()
    {
        GameObject myEvent = GameObject.Find("EventSystem");
        myEvent.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    #endregion
}
