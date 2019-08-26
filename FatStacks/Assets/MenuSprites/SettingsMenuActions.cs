using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SettingsMenuActions : MonoBehaviour
{
    public GameObject mainMenu;
    public Slider sliderSensitivity;
    public Slider sliderFieldOfView;

    private void Start()
    {
        sliderSensitivity.value = PlayerPrefs.GetFloat("Sensitivity");
        sliderFieldOfView.value = PlayerPrefs.GetFloat("FOV");
    }

    public void SensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value);
    }

    public void FieldOfViewChanged(float value)
    {
        PlayerPrefs.SetFloat("FOV", value);
    }

    public void BackPressed()
    {
        PlayerPrefs.Save();
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
