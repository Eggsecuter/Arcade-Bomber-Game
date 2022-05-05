using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LevelClock : MonoBehaviour
{
    public static LevelClock Instance;
    public float StartTime = 1f, IntervalTime = 1f;
    public Player Player;
    public event Action<bool> ClockTick;

    private Image _clockUI = null;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _clockUI = GetComponent<Image>();
        _clockUI.fillAmount = 100;

        InvokeRepeating(nameof(Tick), StartTime, IntervalTime);
    }

    private void Update()
    {
        _clockUI.fillAmount += 1 / IntervalTime * Time.deltaTime;
    }

    private void Tick()
    {
        bool isMoving = Player.stamina > 0 && Input.GetButton("Action");

        ClockTick.Invoke(isMoving);
        _clockUI.fillAmount = 0;
    }
}
