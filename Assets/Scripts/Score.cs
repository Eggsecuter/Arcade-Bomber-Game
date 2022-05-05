using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Score : MonoBehaviour
{
    private Text ScoreUI = null;
    private int score = 0;

    void Start()
    {
        ScoreUI = GetComponent<Text>();

        LevelClock.Instance.ClockTick += UpdateScore;
    }

    private void UpdateScore(bool moved)
    {
        if (!moved)
            return;

        score++;
        ScoreUI.text = score.ToString();
    }
}
