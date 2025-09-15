using UnityEngine;

[CreateAssetMenu(menuName = "MassEvent/NPC Event")]
public class NPCEvent : TileEvent
{
    [TextArea(3, 15)] public string Message;

    public override void Exec(RPGSceneManager manager)
    {
        manager.ShowMessageWindow(Message);
    }
}