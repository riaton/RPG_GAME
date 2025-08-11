using UnityEngine;
using UnityEngine.Tilemaps;

public class MassEvent : ScriptableObject
{
    public TileBase Tile;

    public virtual void Exec(RPGSceneManager manager) { }
}