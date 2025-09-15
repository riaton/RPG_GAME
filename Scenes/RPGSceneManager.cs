using System.Collections;
using UnityEngine;

public class RPGSceneManager : MonoBehaviour
{
    public Player Player;
    Coroutine _currentCoroutine;
    public Vector3Int CurrentEventTilePosition{ get; private set; }
    public Map CurrentMap;
    [SerializeField]Map RestartGameMap;
    [SerializeField]Vector3Int RestartGamePosition;
    [SerializeField, TextArea(3, 5)]string GameOverMessage;
    [SerializeField, TextArea(3, 15)]string GameClearMessage;
    public TitleMenu TitleWindow;
    public Menu PlayerWindow;
    public ItemShopMenu ItemShopWindow;
    [SerializeField] public BattleWindow BattleWindow;
    public MessageWindow MessageWindow;
    [SerializeField]GameClear GameClearWindow;
    //public ItemList ItemList;
    public bool IsPauseScene
    {
        get
        {
            return !MessageWindow.IsEndMessage || PlayerWindow.DoOpen ||
                ItemShopWindow.DoOpen || BattleWindow.DoOpen;
        }
    }

    void Start()
    {
        StartTitle();
    }

    public void StartTitle(){
        StopCurrentCoroutine();
        Player.gameObject.SetActive(false);
        if(CurrentMap != null) CurrentMap.gameObject.SetActive(false);
        TitleWindow.Open();
    }

    public void StartGame(){
        StopCurrentCoroutine();
        TitleWindow.Close();
        Player.gameObject.SetActive(true);
        if(CurrentMap != null) CurrentMap.gameObject.SetActive(true);
        _currentCoroutine = StartCoroutine(MovePlayer());
    }

    void StopCurrentCoroutine(){
        if(_currentCoroutine != null){
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
    }

    IEnumerator MovePlayer()
    {
        while (true)
        {
            if (GetIsMoveKeyPushed(out var move))
            {
                var movedPosition = Player.Position + move;
                var tile = CurrentMap.GetTile(movedPosition);
                Player.SetDirection(move);
                if (tile.isMovable)
                {
                    Player.Position = movedPosition;
                    yield return new WaitWhile(() => Player.IsMoving);

                    if (tile.tileEvent != null)
                    {
                        CurrentEventTilePosition = movedPosition;
                        tile.tileEvent.Exec(this);
                    }
                    else if (CurrentMap.MapEncount != null)
                    {
                        var rnd = new System.Random();
                        var encount = CurrentMap.MapEncount.EncountJudge(rnd);
                        if (encount != null)
                        {
                            BattleWindow.SetUseEncounter(encount);
                            BattleWindow.Open();//SetUseEncounterはいらないのでは(Openでやって仕舞えばいいのでは)
                        }
                    }

                }
                else if (tile.mapObject != null && tile.mapObject.Event != null)
                {
                    CurrentEventTilePosition = movedPosition;
                    tile.mapObject.Event.Exec(this);
                }
            }
            yield return new WaitWhile(() => IsPauseScene);
            if(Player.BattleParameter.HP <= 0){
                StartCoroutine(GameOver());
                yield break;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                OpenPlayerMenu();
            }

            if(Input.GetKeyDown(KeyCode.A)){
                GameClear();
            }
        }
    }

    bool GetIsMoveKeyPushed(out Vector3Int move)
    {
        var moveKeyPushed = false;
        move = Vector3Int.zero;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            move.x -= 1; moveKeyPushed = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            move.x += 1; moveKeyPushed = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            move.y += 1; moveKeyPushed = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            move.y -= 1; moveKeyPushed = true;
        }
        return moveKeyPushed;
    }

    public void ShowMessageWindow(string message)
    {
        MessageWindow.StartMessage(message);
    }

    public void OpenPlayerMenu()
    {
        PlayerWindow.Open();
    }

    public void GameClear(){
        StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(GameClearCoroutine());
    }

    IEnumerator GameClearCoroutine(){
        MessageWindow.StartMessage(GameClearMessage);
        yield return new WaitUntil(() => MessageWindow.IsEndMessage);
        GameClearWindow.StartMessage(GameClearWindow.Message);
        yield return new WaitWhile(() => GameClearWindow.DoOpen);

        _currentCoroutine = null;
        RestartGame();
    }

    IEnumerator GameOver()
    {
        MessageWindow.StartMessage(GameOverMessage);
        yield return new WaitUntil(() => MessageWindow.IsEndMessage);
        RestartGame();
    }

    void RestartGame()
    {
        Object.Destroy(CurrentMap.gameObject);
        CurrentMap = Object.Instantiate(RestartGameMap);
 
        Player.SetPosNoCoroutine(RestartGamePosition);
        Player.CurrentDirection = Direction.Down;
 
        if(_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(MovePlayer());
    }
}