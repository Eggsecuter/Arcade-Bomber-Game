using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public LevelGenerator Generator;
    public GameObject GameOverScreen;
    public GameObject NewHighScoreUI;
    public GameObject HighScoreUI;
    public GameObject ScoreUI;
    public Score score;
    [HideInInspector] public int stamina;

    [SerializeField] private Image staminaUI = null;
    [SerializeField] private Image healthUI = null;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int maxStamina = 3;

    private AudioSource _audioSource = null;
    private Animator _animator;
    private readonly Queue<RowMovement> _movements = new Queue<RowMovement>();
    private RowMovement _movement;
    private int _health;
    [HideInInspector] public bool spawnProtection = true;

    private void Start()
    {
        GameOverScreen.SetActive(false);
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();

        _animator.speed = 1 / LevelClock.Instance.IntervalTime;

        // Initialize stats
        _health = maxHealth;
        stamina = maxStamina;

        // Initialize level
        for (int i = 0; i < Generator.Rows; i++)
        {
            _movements.Enqueue(Generator.NextRow());
        }

        _movement = _movements.Dequeue();

        // Start interaction
        LevelClock.Instance.ClockTick += UpdatePosition;
    }

    private void OnDestroy()
    {
        LevelClock.Instance.ClockTick -= UpdatePosition;
    }

    public void TakeDamage()
    {
        _audioSource.Play();
        _health--;
        healthUI.fillAmount = 1f / maxHealth * _health;

        if (_health <= 0)
        {
            Die();
        }
    }

    private void UpdatePosition(bool moved)
    {
        if (!moved)
        {
            ChangeStamina(false);
            return;
        }

        Generator.pathTilesToPlayer--;
        ChangeStamina(true);

        if (_movement.Distance == 0)
        {
            _movements.Enqueue(
                Generator.NextRow()
            );

            _movement = _movements.Dequeue();
        }
        else
        {
            int direction = _movement.HeadRight ? 1 : -1;
            ((RectTransform)transform).pivot += new Vector2(1f / (direction * (Generator.Columns -1)), 0);

            _movement.Distance -= 1;
        }
    }

    private void ChangeStamina(bool use)
    {
        if (use)
        {
            stamina--;
            spawnProtection = false;
        }
        else if (stamina < maxStamina)
        {
            stamina++;
        }
        else if (!spawnProtection)
        {
            TakeDamage();
        }

        staminaUI.fillAmount = 1f / maxStamina * stamina;
    }

    private void Die()
    {
        int highscore = PlayerPrefs.GetInt("Highscore", 0);

        bool newHighScore = highscore < score.score;
        NewHighScoreUI.SetActive(newHighScore);
        HighScoreUI.SetActive(!newHighScore);
        ScoreUI.GetComponent<Text>().text = $"Score: {score.score}";

        if (newHighScore)
        {
            PlayerPrefs.SetInt("Highscore", score.score);
        }
        else
        {
            HighScoreUI.GetComponent<Text>().text = $"High Score: {highscore}";
        }

        LevelClock.Instance.Pause(false);
        GameOverScreen.SetActive(true);
    }
}
