using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuActions : MonoBehaviour
{
    public string startLevel;
    public GameObject settingsMenu;

    public void StartClicked()
    {
        SceneManager.LoadScene(startLevel);
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
