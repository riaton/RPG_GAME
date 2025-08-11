public class Player : CharacterBase
{
    //外からセットするもの
    public BattleParameter InitialBattleParameter;
    //内部で使用するもの
    public BattleParameterBase BattleParameter; 
    protected override void Start()
    {
        DoMoveCamera = true;
        base.Start();
        InitialBattleParameter.Data.CopyTo(BattleParameter);
    }
}