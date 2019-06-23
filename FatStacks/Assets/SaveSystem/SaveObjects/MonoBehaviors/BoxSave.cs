using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSave : SaveObject
{
    public override void fillSaveData(Save save)
    {
        if(save.boxSaveData == null)
            save.boxSaveData = new List<BoxSaveData>();
        BoxSaveData boxSaveData = new BoxSaveData();
        boxSaveData.Write(save, this);
        save.boxSaveData.Add(boxSaveData);

    }
}
