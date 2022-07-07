using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialHandler : MonoBehaviour
{
    public AudioClip selectSound; 
    public AudioClip navigateSound;
    public GameObject[] pages;

    private AudioSource audioSource;
    private int currentPageIndex = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdatePages();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Horizontal"))
            NavigatePages();

        if (Input.GetButtonDown("Cancel"))
            StartCoroutine(nameof(ReturnToMenu));
    }

    private void NavigatePages()
    {
        bool navigateRight = Input.GetAxisRaw("Horizontal") > 0;

        audioSource.Stop();
        audioSource.PlayOneShot(navigateSound);

        if (navigateRight && currentPageIndex < pages.Length - 1)
        {
            currentPageIndex++;
        }
        else if (!navigateRight && currentPageIndex > 0)
        {
            currentPageIndex--;
        }

        UpdatePages();
    }

    private void UpdatePages()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(currentPageIndex == i);
        }
    }

    private IEnumerator ReturnToMenu()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(selectSound);
        yield return new WaitForSeconds(selectSound.length);

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex - 2
        );
    }
}
