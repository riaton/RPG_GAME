﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class Map : MonoBehaviour
{
    public Grid Grid { get => GetComponent<Grid>(); }
    Dictionary<string, Tilemap> _tilemaps;
    HashSet<CharacterBase> _characters = new();

    readonly static string BACKGROND_TILEMAP_NAME = "Background";
    readonly static string NONE_OBJECTS_TILEMAP_NAME = "NoneObjects";
    readonly static string OBJECTS_TILEMAP_NAME = "Objects";
    readonly static string EVENT_BOX_TILEMAP_NAME = "EventBox";

    [SerializeField] List<MassEvent> _eventChips;
    public MassEvent FindMassEvent(TileBase tile)
    {
        return _eventChips.Find(_c => _c.Tile == tile);
    }

    public void AddCharacter(CharacterBase character){
        if(!_characters.Contains(character) && character != null){
            _characters.Add(character);
        }
    }

    public CharacterBase GetCharacter(Vector3Int pos){
        return _characters.FirstOrDefault(_c => _c.Pos == pos);
    }

    public bool FindMassEventPos(TileBase tile, out Vector3Int pos)
    {
        var eventLayer = _tilemaps[EVENT_BOX_TILEMAP_NAME];
        Debug.Log(_tilemaps.Count);
        var renderer = eventLayer.GetComponent<Renderer>();
        var min = eventLayer.LocalToCell(renderer.bounds.min);
        var max = eventLayer.LocalToCell(renderer.bounds.max);
        pos = Vector3Int.zero;
        for (pos.y = min.y; pos.y < max.y; ++pos.y)
        {
            for (pos.x = min.x; pos.x < max.x; ++pos.x)
            {
                var t = eventLayer.GetTile(pos);
                if (t == tile) return true;
            }
        }
        return false;
    }

    private void Awake()
    {
        _tilemaps = new Dictionary<string, Tilemap>();
        foreach (var tilemap in Grid.GetComponentsInChildren<Tilemap>())
        {
            _tilemaps.Add(tilemap.name, tilemap);
        }

        //EventBoxを非表示にする
        _tilemaps[EVENT_BOX_TILEMAP_NAME].gameObject.SetActive(false);
        
        AddCharacter(Object.FindObjectOfType<Player>());
    }

    public Vector3 GetWorldPos(Vector3Int pos)
    {
        return Grid.CellToWorld(pos);
    }

    public class Mass
    {
        public bool isMovable;
        public TileBase eventTile;
        public MassEvent massEvent;
        public CharacterBase character;
    }
    public Mass GetMassData(Vector3Int pos)
    {
        var mass = new Mass();
        mass.eventTile = _tilemaps[EVENT_BOX_TILEMAP_NAME].GetTile(pos);
        mass.isMovable = true;
        mass.character = GetCharacter(pos);

        if(mass.character != null)
        {
            mass.isMovable = false;
        }
        else if (mass.eventTile != null)
        {
            mass.massEvent = FindMassEvent(mass.eventTile);
        }
        else if (_tilemaps[OBJECTS_TILEMAP_NAME].GetTile(pos))
        {
            mass.isMovable = false;
        }
        else if (_tilemaps[BACKGROND_TILEMAP_NAME].GetTile(pos) == null)
        {
            mass.isMovable = false;
        }
        return mass;
    }
}