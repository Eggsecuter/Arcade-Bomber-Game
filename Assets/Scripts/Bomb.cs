using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Player _player;
    private BombType _type;
    private int _tilesToPlayer;
    private bool _triggered = false;

    private void OnDestroy()
    {
        LevelClock.Instance.ClockTick -= Tick;
    }

    public void Initialize(Player player, int tilesToPlayer, BombType type)
    {
        _player = player;
        _tilesToPlayer = tilesToPlayer;
        _type = type;
        GetComponent<SpriteRenderer>().sprite = _type.Image;

        LevelClock.Instance.ClockTick += Tick;
    }

    private void Tick(bool moved)
    {
        if (moved)
        {
            _tilesToPlayer--;
        }

        if (_triggered || _tilesToPlayer <= _type.TriggerRange)
        {
            _triggered = true;

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
        bool isInPlayerRange = Mathf.Abs(_tilesToPlayer) <= _type.ExplosionRange;

        if (isInPlayerRange)
            _player.TakeDamage();

        Destroy(gameObject);
    }
}
