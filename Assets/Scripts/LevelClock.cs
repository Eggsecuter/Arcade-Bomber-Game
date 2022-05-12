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

    private Animator _animator = null;

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        _animator = GetComponent<Animator>();
        _animator.speed = 0;
        yield return new WaitForSeconds(StartTime);
        _animator.speed = 1 / IntervalTime;

        InvokeRepeating(nameof(Tick), 0f, IntervalTime);
    }

    private void Tick()
    {
        bool isMoving = Player.stamina > 0 && Input.GetButton("Action");

        ClockTick.Invoke(isMoving);
    }
}
