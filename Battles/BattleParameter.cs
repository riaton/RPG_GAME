using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleParameterBase
{
    [Min(1)] public int HP;
    [Min(1)] public int MaxHP;
    [Min(1)] public int Attack;
    [Min(1)] public int Defense;
    [Min(1)] public int Level;
    [Min(0)] public int Exp;
    [Min(0)] public int Money;
    public Weapon Weapon;
    public Armor Armor;
    public bool IsLimitItemCount { get => Items.Count >= 4; }
    public List<Item> Items;
    public int AttackPower { get => Attack + (Weapon != null ? Weapon.AttackPower : 0); }
    public int DefensePower { get => Defense + (Armor != null ? Armor.DefensePower : 0); }
    public bool IsNowDefense { get; set; } = false;
    [Min(1)] public int LimitAttack = 100;
    [Min(1)] public int LimitDefense = 100;
    [Min(1)] public int LimitHP = 500;

    public void AdjustParamWithLevel()
    {
        Level = Exp / 100;
        Attack = (int)(LimitAttack * Level / 100f);
        Defense = (int)(LimitDefense * Level / 100f);
        MaxHP = (int)(LimitHP * Level / 100f);
    }

    public virtual void CopyTo(BattleParameterBase dest)
    {
        dest.HP = HP;
        dest.MaxHP = HP < MaxHP ? MaxHP : HP;
        dest.Attack = Attack;
        dest.Defense = Defense;
        dest.Level = Level;
        dest.Exp = Exp;
        dest.Money = Money;
        dest.Weapon = Weapon;
        dest.Armor = Armor;
        dest.Items = new List<Item>(Items.ToArray());
        dest.LimitAttack = LimitAttack;
        dest.LimitDefense = LimitDefense;
        dest.LimitHP = LimitHP;
    }
    //
    public bool GetExp(int exp)
    {
        Exp += exp;

        if (Exp >= (Level + 1) * 100)
        {
            AdjustParamWithLevel();
            return true;
        }

        return false;
    }
}

[CreateAssetMenu(menuName = "BattleParameter")]
public class BattleParameter : ScriptableObject
{
    public BattleParameterBase Data;
}

[System.Serializable]
public class BattleParameterBaseSaveData
{
    public string paramJson;
    public int attackWeaponIndex;
    public int defenseWeaponIndex;
    public int[] itemsIndex;

    public BattleParameterBaseSaveData(BattleParameterBase param, ItemList itemList)
    {
        paramJson = JsonUtility.ToJson(param);
        attackWeaponIndex = itemList.FindIndex(param.Weapon);
        defenseWeaponIndex = itemList.FindIndex(param.Armor);
        itemsIndex = new int[param.Items.Count];
        for (var i = 0; i < itemsIndex.Length; ++i)
        {
            itemsIndex[i] = itemList.FindIndex(param.Items[i]);
        }
    }

    public BattleParameterBase Load(ItemList itemList)
    {
        var inst = JsonUtility.FromJson<BattleParameterBase>(paramJson);
 
        inst.Weapon = itemList[attackWeaponIndex] as Weapon;
        inst.Armor = itemList[defenseWeaponIndex] as Armor;
        inst.Items = new List<Item>(itemsIndex.Length);
        for(var i=0; i< itemsIndex.Length; ++i)
        {
            inst.Items.Add(itemList[itemsIndex[i]]);
        }
        return inst;
    }
}