using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem : MonoBehaviour {

    private string savePath;
    private static string latestSave;

    public static GameObject playerObject; //used for respawning
    public static Player player;

    public static List<SaveObject> saveObjects = new List<SaveObject>();
    
    void Awake () {

        playerObject = Resources.Load<GameObject>("Character/Prefabs/Character");

        //Set up save directory
        savePath = Application.persistentDataPath + "/saves";
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        DirectoryInfo directory = new DirectoryInfo(savePath);
        FileInfo[] saves = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).ToArray();
        if (saves.Length > 0)
            latestSave = saves[0].Name;
        else
            latestSave = null;
    }

    private void Start()
    {
        //Get player in scene
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Save("quicksave.save");
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            LoadLatest();
        }
    }

    public static void Save(string fileName)
    {
        //Make a save
        Save save = makeSave();

        //Record latest save
        latestSave = fileName;

        //Create file
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/saves/" + fileName;
        FileStream file = File.Create(path);

        //Serialize save
        bf.Serialize(file, save);
        Debug.Log(path);
        file.Close();
    }

    public static void Load(string fileName)
    {
        string path = Application.persistentDataPath + "/saves/" + fileName;
        if (File.Exists(path))
        {
            //Open file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);

            //Deserialize save
            Save save = (Save)bf.Deserialize(file);
            Debug.Log(path + "\nLoaded");
            file.Close();

            //Read save
            clearSceneReadSave(save);
        }
        else
        {
            Debug.Log("File does not exist:\n" + path);
        }

    }

    public void LoadLatest()
    {
        Load(latestSave);
    }

    public static Save makeSave()
    {
        Save save = new Save(saveObjects);
        return save;
    }

    public static void clearSceneReadSave(Save save)
    {
        clearExistingObjects();
        save.read();
        ////Clear existing objects
        //ClearMatchThreeObjects();
        //ClearAmmoObjects();
        //Create Player
        
        ////Create Match Threes
        //foreach (matchThreeObjectInfo obj in save.matchThreeObjectInfos)
        //{
        //    GameObject newObj = Instantiate(matchThreeObjectTypes.types[obj.type], GameObject.Find(obj.room).transform);
        //    MatchThreeObject newObjM3 = newObj.GetComponent<MatchThreeObject>();
        //    newObj.name = obj.name;
        //    newObj.transform.position = obj.position.GetPosition();
        //    newObjM3.match3_group_id = (MatchThreeObject.match3_group_id_names)obj.group;
        //    newObjM3.apply_color();
        //}
        ////Create Ammo Pickups
        //foreach (ammoInfo obj in save.ammoInfos)
        //{
        //    GameObject newObj = Instantiate(AmmoObject);
        //    PickupAmmoInteraction newObjAmmo = newObj.GetComponent<PickupAmmoInteraction>();
        //    newObj.name = obj.name;
        //    newObj.transform.position = obj.position.GetPosition();
        //    newObj.transform.eulerAngles = obj.rotation.GetOrientation();
        //    newObjAmmo.gun = (GunController.gun_type)obj.type;
        //    newObjAmmo.amount = obj.amount;
        //}
        ////Set Door Parameters
        //foreach (doorInfo obj in save.doorInfos)
        //{
        //    DoorInteraction door = GameObject.Find(obj.name).GetComponentInChildren<DoorInteraction>();
        //    door.twistLock(obj.locked);
        //    door.swingDoor(obj.open, false);
        //}
        ////Temporary code for setting references
        //foreach (GameObject tutorial in tutorials)
        //{
        //    tutorial.GetComponent<Tutorial>().GetUIComponents(player.gameObject);
        //}
    }

    private static void clearExistingObjects()
    {
        ////Clear player
        //if (player != null)
        //    Destroy(player.gameObject);
        //else
        //    Debug.Log("Player does not exist.");
        //Clear save objects
        foreach(SaveObject saveObject in saveObjects)
        {
            Destroy(saveObject.gameObject);
        }
    }
}

[System.Serializable]
public class Save
{
    //public List<ammoInfo> ammoInfos { get; private set; } = new List<ammoInfo>();
    //public List<doorInfo> doorInfos { get; private set; } = new List<doorInfo>();
    //public List<matchThreeObjectInfo> matchThreeObjectInfos { get; private set; } = new List<matchThreeObjectInfo>();
    //public List<tutorialInfo> tutorialInfos { get; private set; } = new List<tutorialInfo>();
    //public List<SaveObject> saveObjects;
    public List<SaveData> saveData;
    public PlayerSaveData playerData;
    public List<BoxSaveData> boxSaveData;
    public Save(List<SaveObject> saveObjects)
    {
        saveData = new List<SaveData>();
        foreach (SaveObject saveObject in saveObjects)
        {
            saveObject.fillSaveData(this);
        }
    }
    public void read()
    {
        playerData.Read();
        foreach(BoxSaveData boxSaveDatum in boxSaveData)
        {
            boxSaveDatum.Read();
        }
    }
}