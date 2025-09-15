using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "MassEvent/Move Map Event")]
public class MoveMapEvent : TileEvent
{
    public Map MapTo;
    public TileBase StartTile;
    public Direction StartDirection;

    public override void Exec(RPGSceneManager manager)
    {
        //var saveData = Object.FindObjectOfType<SaveData>();
        //saveData.SaveTemporary(manager.ActiveMap);
        Object.Destroy(manager.ActiveMap.gameObject);
        manager.ActiveMap = Object.Instantiate(MapTo);

        if (manager.ActiveMap.FindTileEventPosition(StartTile, out var pos))
        {
            Debug.Log(pos);
            manager.Player.SetPosNoCoroutine(pos);
            manager.Player.CurrentDirection = StartDirection;
        }
    }
}