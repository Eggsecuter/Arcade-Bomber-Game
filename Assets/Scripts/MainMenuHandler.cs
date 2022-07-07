using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public Text highScoreUI;
    [SerializeField] private MenuHandler menuHandler = null;
    [SerializeField] private GameObject fistSelected = null;

    private void Start()
    {
        int highscore = PlayerPrefs.GetInt("Highscore", 0);
        highScoreUI.text = $"High Score: {highscore}";
    }

    public void Play()
    {
        StartCoroutine(menuHandler.ButtonDelay(
            () =>
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex + 1
            )
        ));
    }

    public void Tutorial()
    {
        StartCoroutine(menuHandler.ButtonDelay(
            () =>
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex + 2
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
