using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuActions : MonoBehaviour
{
    public string startLevel;
    public GameObject settingsMenu;
    public MusicTrack theme;

    public void StartClicked()
    {
        SceneManager.LoadScene(startLevel);
        MusicManager.i.SwitchTrack(theme);
    }

    public void QuitClicked()
    {
        Application.Quit();
    }

    public void SettingsClicked()
    {
        gameObject.SetActive(false);
        settingsMenu.SetActive(true);
    }
}
