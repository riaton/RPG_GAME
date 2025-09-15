using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "MassEvent/TreasureBox Event")]
public class TreasureBoxEvent : TileEvent
{
    public Item Item;
    [TextArea(3, 15)] public string TreasureOpenText;
    [TextArea(3, 15)] public string GetItemText;
    [TextArea(3, 15)] public string CantGetItemText;

    public override void Exec(RPGSceneManager manager)
    {
        if(manager.ActiveMap.FindMapObject(manager.CurrentEventTilePosition) == null) return;
        manager.StartCoroutine(OpenTreasure(manager));
    }

    IEnumerator OpenTreasure(RPGSceneManager manager)
    {
        var treasureBox = manager.ActiveMap.FindMapObject(manager.CurrentEventTilePosition) as TreasureBox;

        var messageWindow = manager.MessageWindow;
        messageWindow.Params = null;
        messageWindow.Effects = null;
        messageWindow.StartMessage(TreasureOpenText);

        yield return new WaitUntil(() => messageWindow.IsEndMessage);

        var playerParameter = manager.Player.BattleParameter;
        if (playerParameter.Items.Count < GameConstants.MAX_ITEM_COUNT)
        {
            messageWindow.Params = new string[] { Item.Name };
            messageWindow.StartMessage(GetItemText);
            yield return new WaitUntil(() => messageWindow.IsEndMessage);
            playerParameter.Items.Add(Item);
            treasureBox.Open();
        }
        else
        {
            messageWindow.StartMessage(CantGetItemText);
            yield return new WaitUntil(() => messageWindow.IsEndMessage);
        }
    }
}