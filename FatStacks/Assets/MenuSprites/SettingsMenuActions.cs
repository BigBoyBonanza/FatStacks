﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SettingsMenuActions : MonoBehaviour
{
    public GameObject mainMenu;
    public Slider slider;

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat("Sensitivity");
    }

    public void SensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value);
    }

    public void BackPressed()
    {
        PlayerPrefs.Save();
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
