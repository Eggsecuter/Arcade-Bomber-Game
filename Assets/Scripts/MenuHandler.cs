using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class MenuHandler : MonoBehaviour
{
    [Header("Menu")]
    public GameObject mainMenu;
    public GameObject optionsMenu;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip navigateSound;
    public AudioClip changeOptionSound;
    public AudioClip selectSound;

    private GameObject lastSelected = null;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(
            mainMenu.transform.GetChild(0).gameObject
        );
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }
        else
        {
            lastSelected = EventSystem.current.currentSelectedGameObject;
        }


        if (Input.GetButtonDown("Vertical"))
        {
            audioSource.Stop();
            audioSource.PlayOneShot(navigateSound);
        }

        if (!mainMenu.activeSelf && Input.GetButtonDown("Horizontal"))
        {
            audioSource.Stop();
            audioSource.PlayOneShot(changeOptionSound);
        }

        if (Input.GetButtonUp("Submit"))
        {
            audioSource.Stop();
            audioSource.PlayOneShot(selectSound);
        }
    }

    public IEnumerator ButtonDelay(Action callback)
    {
        yield return new WaitForSeconds(selectSound.length);
        callback();
    }
}
