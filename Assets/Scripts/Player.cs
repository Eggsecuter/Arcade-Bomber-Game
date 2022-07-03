using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public LevelGenerator Generator;
    [HideInInspector] public int stamina;

    [SerializeField] private Image staminaUI = null;
    [SerializeField] private Image healthUI = null;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int maxStamina = 3;

    private AudioSource _audioSource = null;
    private readonly Queue<RowMovement> _movements = new Queue<RowMovement>();
    private RowMovement _movement;
    private int _health;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

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
            if (stamina < maxStamina)
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
        }
        else if (stamina < maxStamina)
        {
            stamina++;
        }

        staminaUI.fillAmount = 1f / maxStamina * stamina;
    }

    private void Die()
    {
        // Play Game Over Screen

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
