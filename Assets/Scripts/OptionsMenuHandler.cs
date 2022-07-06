using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsMenuHandler : MonoBehaviour
{
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [SerializeField] private AudioMixer audioMixer = null;
    [SerializeField] private MenuHandler menuHandler = null;

    private void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);
    }

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
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void ChangeSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}
