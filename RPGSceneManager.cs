using System.Collections;
using UnityEngine;

public class RPGSceneManager : MonoBehaviour
{
    public ItemShopMenu ItemShopMenu;
    public Player Player;
    public Map ActiveMap;
    public MessageWindow MessageWindow;

    Coroutine _currentCoroutine;
    void Start()
    {
        _currentCoroutine = StartCoroutine(MovePlayer());
    }
    public Menu Menu;
    public void OpenMenu(){
        Menu.Open();
    }

    public void ShowMessageWindow(string message)
    {
        MessageWindow.StartMessage(message);
    }
    public bool IsPauseScene
    {
        get
        {
            return !MessageWindow.IsEndMessage || Menu.DoOpen || ItemShopMenu.DoOpen;
        }
    }

    IEnumerator MovePlayer()
    {
        while(true)
        {
            if (GetArrowInput(out var move))
            {
                var movedPos = Player.Pos + move;
                var massData = ActiveMap.GetMassData(movedPos);
                Player.SetDir(move);
                if(massData.isMovable)
                {
                    Player.Pos = movedPos;
                    yield return new WaitWhile(() => Player.IsMoving);
 
                    if(massData.massEvent != null)
                    {
                        massData.massEvent.Exec(this);
                    }
                }
                else if(massData.character != null && massData.character.Event != null)
                {
                    massData.character.Event.Exec(this);
                }
            }
            yield return new WaitWhile(() => IsPauseScene);
            if(Input.GetKeyDown(KeyCode.Space)){
                OpenMenu();
            }
        }
    }

    bool GetArrowInput(out Vector3Int move)
    {
        var doMove = false;
        move = Vector3Int.zero;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            move.x -= 1; doMove = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            move.x += 1; doMove = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            move.y += 1; doMove = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            move.y -= 1; doMove = true;
        }
        return doMove;
    }
}