using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Score : MonoBehaviour
{
    [HideInInspector] public int score = 0;
    private AudioSource _audioSource;
    private Text ScoreUI = null;
    private Color originalColor;

    void Start()
    {
        ScoreUI = GetComponent<Text>();
        originalColor = ScoreUI.color;
        _audioSource = GetComponent<AudioSource>();

        LevelClock.Instance.ClockTick += UpdateScore;
    }

    private void UpdateScore(bool moved)
    {
        if (!moved)
            return;

        score++;
        ScoreUI.text = score.ToString();

        if (score % 10 == 0)
            StartCoroutine(nameof(IndicateCheckpoint));
    }

    private IEnumerator IndicateCheckpoint()
    {
        _audioSource.Play();
        
        for (int i = 0; i < 4; i++)
        {
            ScoreUI.color = Color.white;
            yield return new WaitForSeconds(.1f);
            ScoreUI.color = originalColor;
            yield return new WaitForSeconds(.1f);
        }
    }
}
