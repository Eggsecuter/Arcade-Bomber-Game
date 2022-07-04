using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuHandler : MonoBehaviour
{
    [SerializeField] private MenuHandler menuHandler = null;

    public void Restart()
    {
        StartCoroutine(menuHandler.ButtonDelay(
            () =>
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            )
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
