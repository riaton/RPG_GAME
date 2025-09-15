using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyGroup")]
public class EnemyGroup : ScriptableObject
{
    [Range(0, 1)] public float EscapeSuccessRate = 0.9f;
    public List<Enemy> Enemies;

    public EnemyGroup Clone()
    {
        var clone = ScriptableObject.CreateInstance<EnemyGroup>();
        clone.Enemies = new List<Enemy>(Enemies.Count);
        foreach (var enemy in Enemies)
        {
            clone.Enemies.Add(enemy.Clone());
        }
        return clone;
    }
}