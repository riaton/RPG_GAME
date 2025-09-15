using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Random Encount")]
public class RandomEncount : ScriptableObject
{
    [System.Serializable]
    public class EncountEnemies
    {
        [Range(0, 1)] public float EncountRate;
        public EnemyGroup Enemies;
    }
    [Range(0, 1)] public float EncountJudgeRate = 0.1f;
    public List<EncountEnemies> EncountEnemiesList;

    public EnemyGroup EncountJudge(System.Random rnd)
    {
        if (EncountJudgeRate < rnd.NextDouble()) return null;
        foreach (var encountEnemies in EncountEnemiesList)
        {
            var t = rnd.NextDouble();
            if (t < encountEnemies.EncountRate)
            {
                return encountEnemies.Enemies;
            }
        }
        return null;
    }
}