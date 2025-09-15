using UnityEngine;
using UnityEngine.Tilemaps;

public class TileEvent : ScriptableObject
{
    public TileBase Tile;
    public virtual void Exec(RPGSceneManager manager) {}
}
