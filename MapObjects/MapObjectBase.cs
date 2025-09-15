using System.Collections;
using UnityEngine;

public class MapObjectBase : MonoBehaviour
{
    Vector3Int InitialPosition{get; set;}
    public bool IsActive{get => gameObject.activeSelf; set => gameObject.SetActive(value);}
    public string IdentityKey{get => $"{gameObject.name}_{GetType().Name}_{InitialPosition}";}
    [Range(0, 2)] public float MoveSecond = 0.1f;
    [SerializeField] protected RPGSceneManager RPGSceneManager;
    Coroutine _moveCoroutine;
    [SerializeField] Vector3Int _position;
    public TileEvent Event;
    public bool IsMoving { get => _moveCoroutine != null; }
    public bool CameraFollow = false;
    [SerializeField] Direction _currentDirection = Direction.Down;
    protected Animator Animator { get => GetComponent<Animator>(); }
    static readonly string TRIGGER_MoveDown = "MoveDownTrigger";
    static readonly string TRIGGER_MoveLeft = "MoveLeftTrigger";
    static readonly string TRIGGER_MoveRight = "MoveRightTrigger";
    static readonly string TRIGGER_MoveUp = "MoveUpTrigger";
    public virtual Direction CurrentDirection
    {
        get => _currentDirection;
        set
        {
            if (_currentDirection == value) return;
            _currentDirection = value;
            SetAnimation(value);
        }
    }
    public virtual Vector3Int Position//
    {
        get => _position;
        set
        {
            if (_position == value) return;//

            if (RPGSceneManager.CurrentMap == null)
            {
                _position = value;
            }
            else
            {
                if (_moveCoroutine != null)
                {
                    StopCoroutine(_moveCoroutine);
                    _moveCoroutine = null;
                }
                _moveCoroutine = StartCoroutine(MoveCoroutine(value));
            }
        }
    }
    
    public virtual void SetPosNoCoroutine(Vector3Int position)//
    {
        if(_moveCoroutine != null)
        {
             StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }
        _position = position;
        transform.position = RPGSceneManager.CurrentMap.Grid.CellToWorld(position);
        MoveCamera();
    }
    
    // gridStep: 現在位置から進もうとする1マス分の座標差分（Vector3Int）
    public virtual void SetDirection(Vector3Int gridStep)
    {
        if (Mathf.Abs(gridStep.x) > Mathf.Abs(gridStep.y))
        {
            CurrentDirection = gridStep.x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            CurrentDirection = gridStep.y > 0 ? Direction.Up : Direction.Down;
        }
    }

    protected void SetAnimation(Direction direction)
    {
        if (Animator == null || Animator.runtimeAnimatorController == null) return;

        string triggerName = direction switch
        {
            Direction.Up => TRIGGER_MoveUp,
            Direction.Down => TRIGGER_MoveDown,
            Direction.Left => TRIGGER_MoveLeft,
            Direction.Right => TRIGGER_MoveRight,
            _ => throw new System.NotImplementedException(""),
        };
        Animator.SetTrigger(triggerName);
    }

    protected IEnumerator MoveCoroutine(Vector3Int position)
    {
        _position = position;//

        var startPos = transform.position;
        var goalPos = RPGSceneManager.CurrentMap.Grid.CellToWorld(position);
        var t = 0f;
        while (t < MoveSecond)
        {
            yield return null;
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, goalPos, t / MoveSecond);
            MoveCamera();
        }
        _moveCoroutine = null;
    }

    protected virtual void Awake()//
    {
        if (RPGSceneManager == null) RPGSceneManager = Object.FindObjectOfType<RPGSceneManager>();
        InitialPosition = Position;
        SetAnimation(_currentDirection);

        var joinedMap = GetJoiningMap();
        if (joinedMap != null)
        {
            joinedMap.AddMapObject(this);
        }
        else
        {
            RPGSceneManager.CurrentMap.AddMapObject(this);
        }
    }

    protected virtual void Start()
    {
        _moveCoroutine = StartCoroutine(MoveCoroutine(Position));
    }

    public Map GetJoiningMap()
    {
        if (transform.parent != null)
        {
            return transform.parent.GetComponent<Map>();
        }
        return null;
    }

    protected void OnValidate()
    {
        if (RPGSceneManager != null && RPGSceneManager.CurrentMap != null)
        {
            transform.position = RPGSceneManager.CurrentMap.Grid.CellToWorld(Position);
        }
        else if (transform.parent != null)
        {
            var map = transform.parent.GetComponent<Map>();
            if (map != null)
            {
                transform.position = map.Grid.CellToWorld(Position);
            }
        }
    }

    private void MoveCamera()
    {
        Vector3 playerPivot = new(0.16f, 0.16f, 0.0f);
        if(CameraFollow == true) Camera.main.transform.position = transform.position + Vector3.forward * -10 + playerPivot;
    }

/**
    public virtual InstantSaveData GetInstantSaveData(){
        return new InstantSaveData(this);
    }

    [System.Serializable]
    public class InstantSaveData{
        public string id;
        public Vector3Int Position;
        public Direction Direction;
        public InstantSaveData(){}
        public InstantSaveData(CharacterBase character){
            id = character.IdentityKey;
            Position = character.Position;
            Direction = character.CurrentDirection;
        }
    }

    [System.Serializable]
    public class SaveData{
        public string id;
        public SaveData(){}
        public SaveData(CharacterBase character){
            id = character.IdentityKey;
        }
    }
    public virtual SaveData GetSaveData(){
        return null;
    }
    public virtual void LoadInstantSaveData(string saveDataJson)
    {
        var saveData = JsonUtility.FromJson<InstantSaveData>(saveDataJson);
        SetPosNoCoroutine(saveData.Position);
        CurrentDirection = saveData.Direction;
    }
 
    public virtual void LoadSaveData(string saveDataJson){}
    **/
}
