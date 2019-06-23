using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoxSaveData : SaveData {
    public string name;
    public string resourcePath;
    public int group;
    public SerializableVector3 position;
    public string room;
    public override void Write(Save save, SaveObject saveObject)
    {
        //Get box
        Box box = saveObject.GetComponent<Box>();

        //Set name
        name = box.name;

        //Set group and type
        resourcePath = box.resourcePath;
        group = (int)box.match3_group_id;

        //Set position
        position = box.transform.position;

        //Set room
        room = box._Grid.name;
    }

    public override void Read()
    {
        Box box = UnityEngine.GameObject.Instantiate(Resources.Load<GameObject>(resourcePath), GameObject.Find(room).transform).GetComponent<Box>();
        box.name = name;
        box.transform.position = position;
        box.match3_group_id = (Box.match3_group_id_names)group;
        box.apply_color();
    }
}
