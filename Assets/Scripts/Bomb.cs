using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    private Player _player;
    private BombType _type;
    private int _tilesToPlayer;
    private bool _triggered = false;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        LevelClock.Instance.ClockTick -= Tick;
    }

    public void Initialize(Player player, int tilesToPlayer, BombType type)
    {
        _player = player;
        _tilesToPlayer = tilesToPlayer;
        _type = type;
        GetComponent<Image>().sprite = _type.Image;

        LevelClock.Instance.ClockTick += Tick;
    }

    private void Tick(bool moved)
    {
        if (moved)
        {
            _tilesToPlayer--;
        }

        if (!_triggered && _tilesToPlayer <= _type.TriggerRange)
        {
            _triggered = true;
            _animator.SetBool("IsTicking", true);
        }
        
        if (_triggered)
        {
            if (_type.TicksToExplosion == 0)
            {
                Explode();
            }
            else
            {
                _type.TicksToExplosion--;
            }
        }
    }

    private void Explode()
    {
        if (_tilesToPlayer == 0)
            _player.TakeDamage();

        _animator.SetBool("IsExploding", true);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
