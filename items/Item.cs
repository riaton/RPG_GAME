using UnityEngine;
using System.Collections.Generic;

public class Item : ScriptableObject
{
    public string Name;
    public string Description;
    public int Price;
    public virtual void Use(BattleParameterBase target) {}
}

public class ItemList : ScriptableObject
{
    [SerializeField] List<Item> List;

    public int FindIndex(Item item)
    {
        return List.IndexOf(item);
    }

    public Item this[int index]
    {
        get => index != -1 ? List[index] : null;
    }
}