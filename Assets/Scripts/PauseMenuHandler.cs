using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour
{
    [SerializeField] private MenuHandler menuHandler = null;
    [SerializeField] private GameObject fistSelected = null;

    public void Resume()
    {
        StartCoroutine(menuHandler.ButtonDelay(
            LevelClock.Instance.Resume
        ));
    }

    public void Restart()
    {
        StartCoroutine(menuHandler.ButtonDelay(
            () =>
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
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

    public void ReturnToMenu()
    {
        StartCoroutine(menuHandler.ButtonDelay(
            () => SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex - 1
            )
        ));
    }
}
