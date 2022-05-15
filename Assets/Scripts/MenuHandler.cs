using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuHandler : MonoBehaviour
{
    public Button[] menuButtons;
    public AudioClip navigateSound;
    public AudioClip selectSound;
    public AudioSource audioSource;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        EventSystem.current.SetSelectedGameObject(menuButtons[0].gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            audioSource.Stop();
            audioSource.PlayOneShot(navigateSound);
        }

        if (Input.GetButtonUp("Submit"))
        {
            audioSource.Stop();
            audioSource.PlayOneShot(selectSound);
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex + 1
        );
    }

    public void OpenSettings()
    {
        print("Not implemented yet");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
