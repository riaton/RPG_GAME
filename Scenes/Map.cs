using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    public RandomEncount MapEncount;
    Dictionary<string, Tilemap> _tilemaps;
    [SerializeField] List<TileEvent> _tileEvents;
    HashSet<MapObjectBase> _mapObjects = new();
    public Grid Grid { get => GetComponent<Grid>(); }
    readonly static string BACKGROND_TILEMAP_NAME = "Background";
    readonly static string OBJECTS_TILEMAP_NAME = "Objects";
    readonly static string EVENT_BOX_TILEMAP_NAME = "EventBox";

    private void Start()
    {
        //var saveData = Object.FindObjectOfType<GlobalSaveData>();
        //saveData.LoadSaveData(this);
    }

    private void Awake()
    {
        _tilemaps = new Dictionary<string, Tilemap>();
        foreach (var tilemap in Grid.GetComponentsInChildren<Tilemap>())
        {
            _tilemaps.Add(tilemap.name, tilemap);
            var renderer = tilemap.GetComponent<Renderer>();
            tilemap.LocalToCell(renderer.bounds.min);
            tilemap.LocalToCell(renderer.bounds.max);
        }
        //EventBoxを非表示にする
        _tilemaps[EVENT_BOX_TILEMAP_NAME].gameObject.SetActive(false);
        AddMapObject(Object.FindObjectOfType<Player>());
    }

    public void AddMapObject(MapObjectBase mapObject)
    {
        if (!_mapObjects.Contains(mapObject) && mapObject != null)
        {
            _mapObjects.Add(mapObject);
        }
    }

    public MapObjectBase FindMapObject(Vector3Int position)
    {
        return _mapObjects.Where(_c => _c.IsActive).FirstOrDefault(_c => _c.Position == position);
    }

    public TileEvent FindTileEvent(TileBase tile)
    {
        return _tileEvents.Find(_c => _c.Tile == tile);
    }

    public bool FindTileEventPosition(TileBase tile, out Vector3Int position)
    {
        var eventLayer = _tilemaps[EVENT_BOX_TILEMAP_NAME];
        var renderer = eventLayer.GetComponent<Renderer>();
        var min = eventLayer.LocalToCell(renderer.bounds.min);
        var max = eventLayer.LocalToCell(renderer.bounds.max);
        position = Vector3Int.zero;
        for (position.y = min.y; position.y < max.y; ++position.y)
        {
            for (position.x = min.x; position.x < max.x; ++position.x)
            {
                var t = eventLayer.GetTile(position);
                if (t == tile) return true;
            }
        }
        return false;
    }

    public Tile GetTile(Vector3Int position)
    {
        var tile = new Tile();
        tile.eventBoxTile = _tilemaps[EVENT_BOX_TILEMAP_NAME].GetTile(position);
        tile.isMovable = true;
        tile.mapObject = FindMapObject(position);

        if (tile.mapObject != null)
        {
            tile.isMovable = false;
        }
        else if (tile.eventBoxTile != null)
        {
            tile.tileEvent = FindTileEvent(tile.eventBoxTile);
        }
        else if (_tilemaps[OBJECTS_TILEMAP_NAME].GetTile(position))
        {
            tile.isMovable = false;
        }
        else if (_tilemaps[BACKGROND_TILEMAP_NAME].GetTile(position) == null)
        {
            tile.isMovable = false;
        }
        return tile;
    }

/**
    public InstantSaveData GetInstantSaveData(){
        var saveData = new InstantSaveData();
        saveData.characters = _characters.Select(_c => _c.GetInstantSaveData()).Where(_s => _s != null).Select(_s => JsonUtility.ToJson(_s)).ToList();
        return saveData;
    }

    [System.Serializable]
    public class InstantSaveData{
        public List<string> characters = new List<string>();
    }

    public MapObjectBase GetCharacterId(string id)
    {
        return _mapObjects.FirstOrDefault(_c => _c.IdentityKey == id);
    }

    public void Load(InstantSaveData saveData)
    {
        if (saveData.characters != null)
        {
 
            foreach (var json in saveData.characters)
            {
                var data = JsonUtility.FromJson<MapObjectBase.SaveData>(json);
                if (data == null) continue;
 
                var ch = GetCharacterId(data.id);
                if (ch != null)
                {
                    ch.LoadInstantSaveData(json);
                }
                else
                {
                    Debug.LogError($"Don't found character={data.id}....");
                }
            }
        }
    }
 
    public void Load(SaveData saveData)
    {
        if(saveData.characters != null)
        {
            foreach (var json in saveData.characters)
            {
                var data = JsonUtility.FromJson<MapObjectBase.SaveData>(json);
                if (data == null) continue;
 
                var ch = GetCharacterId(data.id);
                if (ch != null)
                {
                    ch.LoadSaveData(json);
                }
                else
                {
                    Debug.LogError($"Don't found character={data.id}...");
                }
            }
        }
    }

    [System.Serializable]
    public class SaveData{
        public List<string> characters = new List<string>();
    }
    public SaveData GetSaveData(){
        var saveData = new SaveData();
        saveData.characters = _characters.Where(_c => !(_c is Player)).Select(_c => _c.GetSaveData()).Where(_s => _s != null).Select(_s => JsonUtility.ToJson(_s)).ToList();
        return saveData;
    }
    **/
}

public class Tile
{
    public bool isMovable;
    public TileBase eventBoxTile;
    public TileEvent tileEvent;
    public MapObjectBase mapObject;
}