using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelClock : MonoBehaviour
{
    public static LevelClock Instance;

    private readonly int _startTime = 1, _intervalTime = 1;

    public Image ClockImage;
    public event Action<bool> Moved;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ClockImage.fillAmount = 0;

        InvokeRepeating(nameof(Tick), _startTime, _intervalTime);
    }

    private void Update()
    {
        ClockImage.fillAmount += 1 / _intervalTime * Time.deltaTime;
    }

    private void Tick()
    {
        Moved.Invoke(Input.GetButton("Action"));
        ClockImage.fillAmount = 0;
    }
}
