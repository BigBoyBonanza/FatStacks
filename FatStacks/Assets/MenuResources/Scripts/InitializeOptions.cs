using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeOptions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AddKeyIfDoesNotExist("Sensitivity", 1f);
        AddKeyIfDoesNotExist("FOV", 90f);
    }

    public void AddKeyIfDoesNotExist(string key, float value)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetFloat(key, value);
        }
    }
}
