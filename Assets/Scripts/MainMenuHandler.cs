using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private MenuHandler menuHandler = null;
    [SerializeField] private GameObject fistSelected = null;

    public void Play()
    {
        StartCoroutine(menuHandler.ButtonDelay(
            () =>
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex + 1
            )
        ));
    }

    public void ActivateOptionsMenu()
    {
        StartCoroutine(menuHandler.ButtonDelay(
            () => {
                menuHandler.optionsMenu.SetActive(true);

                EventSystem.current.SetSelectedGameObject(fistSelected);

                menuHandler.mainMenu.SetActive(false);
            }
        ));
    }

    public void Quit()
    {
        StartCoroutine(menuHandler.ButtonDelay(
            Application.Quit
        ));
    }
}
