using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class SettingsMenuActions : MonoBehaviour
{
    public GameObject mainMenu;
    public Slider sliderSensitivity;
    public Slider sliderFieldOfView;
    public Text textSensitivity;
    public Text textFOV;

    private void Start()
    {
        sliderSensitivity.value = PlayerPrefs.GetFloat("Sensitivity");
        sliderFieldOfView.value = PlayerPrefs.GetFloat("FOV");
    }

    public void SensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value);
        textSensitivity.text = Math.Round(value, 2, MidpointRounding.AwayFromZero).ToString();
    }

    public void FieldOfViewChanged(float value)
    {
        PlayerPrefs.SetFloat("FOV", value);
        textFOV.text = Math.Round(value, 2, MidpointRounding.AwayFromZero).ToString();
    }

    public void BackPressed()
    {
        PlayerPrefs.Save();
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
