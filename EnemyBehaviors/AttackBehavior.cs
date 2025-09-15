using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Behaviors/Attack")]
public class AttackBehavior : EnemyBehaviorBase
{
    protected override void Action(Enemy enemy, BattleWindow battleWindow, TurnInfo outTurnInfo)
    {
        outTurnInfo.Message = $"{enemy.Name}の攻撃!!";
        outTurnInfo.Action = () =>
        {
            enemy.Attack(battleWindow.Player, out AttackResult result);
            var messageWindow = battleWindow.GetRPGSceneManager.MessageWindow;
            var resultMsg = $"プレイヤーは{result.Damage}を受けた...";
            messageWindow.Params = null;
            messageWindow.StartMessage(resultMsg);
        };
    }
}