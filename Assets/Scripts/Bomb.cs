using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Min(0)]
    public int TriggerRange = 0;
    [Min(1)]
    public int TicksToExplosion = 1;
    [Min(0)]
    public int ExplosionRange = 0;

    public int TilesToPlayer { private get; set; }

    private bool _triggered = false;

    private void Start()
    {
        LevelClock.Instance.Moved += Tick;
    }

    private void OnDestroy()
    {
        LevelClock.Instance.Moved -= Tick;
    }

    private void Tick(bool moved)
    {
        if (moved)
        {
            TilesToPlayer--;
        }

        if (_triggered || TilesToPlayer <= TriggerRange)
        {
            _triggered = true;

            if (TicksToExplosion == 0)
            {
                Explode();
            }
            else
            {
                TicksToExplosion--;
            }
        }
    }

    private void Explode()
    {
        bool isInPlayerRange = Mathf.Abs(TilesToPlayer) <= ExplosionRange;

        Destroy(gameObject);
    }
}
