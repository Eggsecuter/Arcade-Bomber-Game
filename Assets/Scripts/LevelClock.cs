using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LevelClock : MonoBehaviour
{
    public static LevelClock Instance;
    public GameObject PauseOverlay;
    public float StartTime = 1f, IntervalTime = 1f;
    public Player Player;
    public event Action<bool> ClockTick;

    private bool isPaused = false;
    private Animator _animator = null;
    private AudioSource _audioSource = null;
    private float _lastTick = 0f;

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        PauseOverlay.SetActive(false);
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _animator.speed = 0;
        yield return new WaitForSeconds(StartTime);

        if (!isPaused)
        {
            _animator.speed = 1 / IntervalTime;
            InvokeRepeating(nameof(Tick), 0f, IntervalTime);
        }
    }

    private void Update()
    {
        if (!isPaused && (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.P)))
            Pause();
    }

    public void Resume()
    {
        InvokeRepeating(nameof(Tick), Time.time % IntervalTime * IntervalTime, IntervalTime);
        _animator.speed = 1 / IntervalTime;
        PauseOverlay.SetActive(false);
        isPaused = false;
    }

    public void Pause(bool pauseOverlay = true)
    {
        CancelInvoke(nameof(Tick));
        _animator.speed = 0;
        PauseOverlay.SetActive(pauseOverlay);
        isPaused = true;
    }

    private void Tick()
    {
        _lastTick = Time.time;
        bool isMoving = Player.stamina > 0 && Input.GetButton("Action");
        _audioSource.Play();
        ClockTick.Invoke(isMoving);
    }
}
