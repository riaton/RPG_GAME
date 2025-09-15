using UnityEngine;

public class Player : MapObjectBase
{
    public BattleParameter InitialBattleParameter;
    public BattleParameterBase BattleParameter;//Baseを参照しないようにする。CharacterParameterみたいなクラスを作る
    //そこら辺のパラメータ設計をきちんとする

    protected override void Start()
    {
        CameraFollow = true;
        base.Start();
        InitialBattleParameter.Data.CopyTo(BattleParameter);
    }
    
    public virtual bool Attack(BattleParameterBase target, out AttackResult result)
    {
        result = new AttackResult();

        result.Damage = Mathf.Max(0, BattleParameter.AttackPower - target.DefensePower);
        if (target.IsNowDefense)
        {
            result.Damage /= 2;
        }
        target.HP -= result.Damage;
        result.LeaveHP = target.HP;
        return target.HP <= 0;
    }

/**
    [System.Serializable]
    public class PlayerSaveData : SaveData{
        public BattleParameterBaseSaveData battleParameter;
        public PlayerSaveData(){}
        public PlayerSaveData(Player character, RPGSceneManager manager){
            battleParameter = new BattleParameterBaseSaveData(character.BattleParameter, manager.ItemList);
        }
    }
    public override SaveData GetSaveData(){
        return new PlayerSaveData(this, RPGSceneManager);
    }
    public override void LoadSaveData(string saveDataJson)
    {
        var saveData = JsonUtility.FromJson<PlayerSaveData>(saveDataJson);
        BattleParameter = saveData.battleParameter.Load(RPGSceneManager.ItemList);
    }
    **/
}