using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Level/Bomb")]
public class BombType : ScriptableObject
{
    public string Name;
    [Range(5, 50)]
    public int SpawnProbability; // on average spawns every (100 / amount) tile
    public int TriggerRange;
    public int TicksToExplosion;
    public int ExplosionRange;
    public Sprite Image;

    public BombType GetInstance()
    {
        var typeClone = CreateInstance<BombType>();
        typeClone.Name = Name;
        typeClone.SpawnProbability = SpawnProbability;
        typeClone.TriggerRange = TriggerRange;
        typeClone.TicksToExplosion = TicksToExplosion;
        typeClone.ExplosionRange = ExplosionRange;
        typeClone.Image = Image;

        return typeClone;
    }
}
