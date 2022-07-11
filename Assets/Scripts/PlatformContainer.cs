using System.Collections.Generic;
using System.Linq;
using UnityEngine;

sealed class PlatformContainer : MonoBehaviour
{
    public static PlatformContainer Instance { get; private set; }

    [SerializeField] PlatformBase _startPlatform;
    [SerializeField] int _initialGeneratePlatformCount;

    [SerializeField] float _spawnPlatformOffsetYmin;
    [SerializeField] float _spawnPlatformOffsetYmax;

    [SerializeField] float _spawnPlatformOffsetX;

    [SerializeField] float _platformWidthMin;
    [SerializeField] float _platformWidthMax;
    [SerializeField] float _platformHeight;

    [SerializeField] float _firstGeneratedPlatformOffsetY;

    private ListQueue<PlatformBase> _platforms;
    private List<IMovingPlatform> _movingPlatforms;
    private bool _trapGenerated;
    private float _initialSpawnPositionY;

    public float LastSpawnPositionY { get; private set; }

    private void Awake()
    {
        Instance = this;

        // Prevent trap from spawn as first platform
        _trapGenerated = true;
        _movingPlatforms = new List<IMovingPlatform>();

        _platforms = new ListQueue<PlatformBase>();
        _platforms.Enqueue(_startPlatform);
        _startPlatform.Renderer.enabled = false;

        LastSpawnPositionY = _startPlatform.transform.position.y + _firstGeneratedPlatformOffsetY;
        _initialSpawnPositionY = LastSpawnPositionY;
    }

    private void Update()
    {
        for (int i = 0; i < _platforms.Count; i++)
        {
            var p = _platforms.Peek();
            if (GameController.Instance.GamePivot.position.y - p.transform.position.y > GameController.Instance.OffsetYFromPivotToRemove)
            {
                _platforms.Dequeue();
                if (p is MovingPlatform)
                {
                    _movingPlatforms.Remove((MovingPlatform)p);
                }

                var type = p.GetType();
                PoolContainer.Instance.Pool(p.Type, p);
            }
            else
            {
                break;
            }
        }

        foreach (var p in _movingPlatforms)
        {
            p.Move();
        }

        var distance = LastSpawnPositionY - PlayerController.Instance.CurrentPositionY;
        if (LastSpawnPositionY - PlayerController.Instance.CurrentPositionY <= GameController.Instance.OffsetYFromPivotToRemove 
            && LastSpawnPositionY < GameController.Instance.LvlGoal)
        {
            GeneratePlatform();
        }
    }

    private void AddPlatform(PlatformBase p)
    {
        p.SetPosition(new Vector3(
            Random.Range(-_spawnPlatformOffsetX, _spawnPlatformOffsetX),
            Random.Range(_spawnPlatformOffsetYmin, _spawnPlatformOffsetYmax) + LastSpawnPositionY,
            transform.position.z));

        LastSpawnPositionY = p.transform.position.y;
        p.SetSize(Random.Range(_platformWidthMin, _platformWidthMax), _platformHeight);
        _platforms.Enqueue(p);

        if (p is MovingPlatform)
        {
            _movingPlatforms.Add((MovingPlatform)p);
        }
    }

    public void InitPlatformGeneration()
    {
        _startPlatform.Renderer.enabled = true;

        for (int i = 0; i < _initialGeneratePlatformCount; i++)
        {
            GeneratePlatform();
        }
    }

    public void GeneratePlatform()
    {
        var type = GameEntityType.CommonPlatform;
        var random = Random.value;
        if (random < 0.25f)
        {
            type = GameEntityType.CommonPlatform;
            _trapGenerated = false;
        }
        else if (random >= 0.25f && random < 0.5f)
        {
            type = GameEntityType.MovingPlatform;
            _trapGenerated = false;
        }
        else if (!_trapGenerated)
        {
            type = GameEntityType.TrapPlatform;
            _trapGenerated = true;
        }
        else
        {
            type = GameEntityType.CommonPlatform;
            _trapGenerated = false;
        }

        var obj = PoolContainer.Instance.Get(type) as PlatformBase;
        obj.Collider.enabled = true;
        AddPlatform(obj);
    }

    public void StopOverseePlatform(PlatformBase platform)
    {
        if (_platforms.Contains(platform))
        {
            _platforms.Remove(platform);
        }
    }

    public void Restart()
    {
        foreach (var p in _platforms)
        {
            PoolContainer.Instance.Pool(p.Type, p);
        }

        _platforms.Clear();
        _movingPlatforms.Clear();
        _trapGenerated = true;
        LastSpawnPositionY = _initialSpawnPositionY;

        _platforms.Enqueue(PoolContainer.Instance.Get(GameEntityType.StartPlatform) as PlatformBase);
    }

    public void Stop()
    {
        Restart();
        _startPlatform.Renderer.enabled = false;
    }
}
