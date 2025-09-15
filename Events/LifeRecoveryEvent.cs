using UnityEngine;


[CreateAssetMenu(menuName = "MassEvent/Life Recovery Event")]
public class LifeRecoveryEvent : TileEvent
{
    [Min(0)] public int Price = 50;
    [TextArea(3, 15)] public string Message;
    [TextArea(3, 15)] public string RecoveryMessage;
    [TextArea(3, 15)] public string MoneyNotEnoughMessage;
    [TextArea(3, 15)] public string CanceledMessage;

    public override void Exec(RPGSceneManager manager)
    {
        var messageWindow = manager.MessageWindow;
        var yesNoMenu = messageWindow.YesNoMenu;
        yesNoMenu.YesAction = () =>
        {
            var playerParameter = manager.Player.BattleParameter;
            if (playerParameter.Money - Price >= 0)
            {
                playerParameter.HP = playerParameter.MaxHP;
                playerParameter.Money -= Price;
                messageWindow.StartMessage(RecoveryMessage);
            }
            else
            {
                messageWindow.Params = new string[] { playerParameter.Money.ToString() };
                messageWindow.StartMessage(MoneyNotEnoughMessage);
            }
        };

        yesNoMenu.NoAction = () =>
        {
            messageWindow.Params = null;
            messageWindow.StartMessage(CanceledMessage);
        };

        messageWindow.Params = new string[] { Price.ToString() };
        manager.ShowMessageWindow(Message);
    }
}