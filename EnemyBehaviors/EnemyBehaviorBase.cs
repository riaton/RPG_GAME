using UnityEngine;

public class EnemyBehaviorBase : ScriptableObject
{
    public int Turn { get; set; }

    public virtual EnemyBehaviorBase Clone()
    {
        var clone = ScriptableObject.CreateInstance(GetType()) as EnemyBehaviorBase;
        return clone;
    }

    public TurnInfo BattleAction(Enemy enemy, BattleWindow battleWindow)
    {
        Turn++;
        var info = new TurnInfo();
        Action(enemy, battleWindow, info);
        return info;
    }

    protected virtual void Action(Enemy enemy, BattleWindow battleWindow, TurnInfo outTurnInfo){}
}