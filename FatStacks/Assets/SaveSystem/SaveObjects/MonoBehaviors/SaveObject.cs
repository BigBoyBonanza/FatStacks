using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject : MonoBehaviour {

    public void Awake()
    {
        Add();
    }
    private void OnDestroy()
    {
        Remove();
    }
    public virtual void Add()
    {
        if (!SaveSystem.saveObjects.Exists(Equals))
        {
            SaveSystem.saveObjects.Add(this);
        }
    }
    public virtual void Remove()
    {
        if (SaveSystem.saveObjects.Exists(Equals))
        {
            SaveSystem.saveObjects.Remove(this);
        }
    }
    public virtual void fillSaveData(Save save)
    {

    }
}
