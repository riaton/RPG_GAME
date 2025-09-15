using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "MassEvent/Boss Event")]
public class BossEvent : TileEvent
{
    public EnemyGroup BossGroup;

    public override void Exec(RPGSceneManager manager)
    {
        if(manager.ActiveMap.FindMapObject(manager.CurrentEventTilePosition) == null) return;
        manager.StartCoroutine(Battle(manager));
    }

    IEnumerator Battle(RPGSceneManager manager)
    {
        var Boss = manager.ActiveMap.FindMapObject(manager.CurrentEventTilePosition) as Boss;
        var battleWindow = manager.BattleWindow;
        battleWindow.SetUseEncounter(BossGroup);
        battleWindow.Open();//

        yield return new WaitWhile(() => battleWindow.DoOpen);//IsOpenへ

        if(manager.Player.BattleParameter.HP > 0)
        {
            Boss.Kill();
        }
    }
}