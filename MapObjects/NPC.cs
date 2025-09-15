using System.Collections;
using UnityEngine;

public class NPC : MapObjectBase
{
    [SerializeField, Range(0.1f, 5f)] float WaitSecond = 1f;
    public bool Movable = true;

    protected override void Start()
    {
        base.Start();
        if(!Movable) return;
        Debug.Log("zzz");
        StartCoroutine(RandomMove());
    }

    IEnumerator RandomMove()
    {
        var rnd = new System.Random();
        while (true)
        {
            yield return new WaitWhile(() => !RPGSceneManager.IsPauseScene);//会話直後に移動させなくするためのもの
            var waitSecond = WaitSecond * (float)rnd.NextDouble();
            yield return new WaitForSeconds(waitSecond);

            yield return new WaitWhile(() => RPGSceneManager.IsPauseScene);//会話直後に移動させなくするためのもの
            Debug.Log("bbb");
            var move = Vector3Int.zero;
            switch (rnd.Next() % 4)
            {
                case 0: move.x = -1; break;
                case 1: move.x = 1; break;
                case 2: move.y = -1; break;
                case 3: move.y = 1; break;
            }

            var movedPos = Position + move;
            SetDirection(move);
            if (RPGSceneManager.CurrentMap != null)
            {
                var toTile = RPGSceneManager.CurrentMap.GetTile(movedPos);
                if (toTile.isMovable)
                {
                    Position = movedPos;
                }
            }
            else
            {
                Position += move;
            }
            yield return new WaitWhile(() => IsMoving);
        }
    }
}