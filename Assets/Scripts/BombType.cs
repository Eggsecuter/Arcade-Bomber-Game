using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Level/Bomb")]
public class BombType : ScriptableObject
{
    [Range(0, 2)]
    public int TriggerRange;
    [Min(1)]
    public int TicksToExplosion;
    public Sprite Image;

    public BombType GetInstance()
    {
        var typeClone = CreateInstance<BombType>();
        typeClone.TriggerRange = TriggerRange;
        typeClone.TicksToExplosion = TicksToExplosion;
        typeClone.Image = Image;

        return typeClone;
    }
}
