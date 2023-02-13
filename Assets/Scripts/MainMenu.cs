using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelSelect;
    [SerializeField] private GameObject settingsMenu;

    private void Start()
    {
        if (!mainMenu.activeSelf)
        {
            mainMenu.SetActive(true);
        }
        if (levelSelect.activeSelf)
        {
            levelSelect.SetActive(false);
        }
        if (settingsMenu.activeSelf)
        {
            settingsMenu.SetActive(false);
        }
    }

    public void ToggleMainMenu()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
    }
    
    public void ToggleLevelSelect()
    {
        levelSelect.SetActive(!levelSelect.activeSelf);
    }
    
    public void ToggleSettingsMenu()
    {
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }
}
