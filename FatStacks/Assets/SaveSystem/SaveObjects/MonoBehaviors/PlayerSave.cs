using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSave : SaveObject {


    public override void fillSaveData(Save save)
    {
        save.playerData = new PlayerSaveData();
        save.playerData.Write(save, this);
    }
}
