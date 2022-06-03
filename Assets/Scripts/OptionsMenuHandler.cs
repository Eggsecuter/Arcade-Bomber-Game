using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class OptionsMenuHandler : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer = null;
    [SerializeField] private MenuHandler menuHandler = null;

    public void ActivateMainMenu()
    {
        StartCoroutine(menuHandler.ButtonDelay(
            () => {
                menuHandler.mainMenu.SetActive(true);

                EventSystem.current.SetSelectedGameObject(
                    menuHandler.mainMenu.transform.GetChild(0).gameObject
                );

                menuHandler.optionsMenu.SetActive(false);
            }
        ));
    }

    public void ChangeMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void ChangeSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }
}
