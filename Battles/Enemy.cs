using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy")]
public class Enemy : ScriptableObject
{
    public string Name;
    public BattleParameterBase BattleParameter;
    public Sprite Sprite;
    public EnemyBehaviorBase EnemyBehavior;

    public virtual Enemy Clone()
    {
        var clone = ScriptableObject.CreateInstance<Enemy>();
        clone.BattleParameter = new BattleParameterBase();
        BattleParameter.CopyTo(clone.BattleParameter);
        clone.Name = Name;
        clone.Sprite = Sprite;
        clone.EnemyBehavior = EnemyBehavior.Clone();
        return clone;
    }

    public virtual TurnInfo BattleAction(BattleWindow battleWindow)
    {
        return EnemyBehavior.BattleAction(this, battleWindow);
    }

    public virtual bool Attack(BattleParameterBase target, out AttackResult result)
    {
        result = new AttackResult();

        result.Damage = Mathf.Max(0, BattleParameter.AttackPower - target.DefensePower);
        if (target.IsNowDefense)
        {
            result.Damage /= 2;
        }
        target.HP -= result.Damage;
        result.LeaveHP = target.HP;
        return target.HP <= 0;
    }
}