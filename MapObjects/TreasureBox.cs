public class TreasureBox : MapObjectBase
{
    public void Open()
    {
        IsActive = false;
    }

/**
    public class TreasureSaveData : SaveData
    {
        public bool DoOpen;
        public TreasureSaveData(TreasureBox self) : base(self)
        {
            DoOpen = self.IsActive;
        }
    }

    public override SaveData GetSaveData()
    {
        return new TreasureSaveData(this);
    }

    public override void LoadSaveData(string saveDataJson)
    {
        base.LoadSaveData(saveDataJson);
 
        var saveData = JsonUtility.FromJson<TreasureSaveData>(saveDataJson);
        if (saveData == null) return;
 
        IsActive = saveData.DoOpen;
    }
    **/
}