
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class PoolContainer : MonoBehaviour
{
    public static PoolContainer Instance;

    [SerializeField] PlayerInstance _playerInstancePrefab;
    [SerializeField] Platform _platformPrefab;
    [SerializeField] MovingPlatform _movingPlatformPrefab;
    [SerializeField] TrapPlatform _trapPrefab;

    private Dictionary<GameEntityType, Stack<MonoBehaviour>> _pool;
    private Dictionary<GameEntityType, MonoBehaviour> _prefabs;

    private void Awake()
    {
        Instance = this;

        _pool = new Dictionary<GameEntityType, Stack<MonoBehaviour>>();

        _prefabs = new Dictionary<GameEntityType, MonoBehaviour>();
        _prefabs.Add(GameEntityType.Player, _playerInstancePrefab);
        _prefabs.Add(GameEntityType.CommonPlatform, _platformPrefab);
        _prefabs.Add(GameEntityType.MovingPlatform, _movingPlatformPrefab);
        _prefabs.Add(GameEntityType.TrapPlatform, _trapPrefab);
    }

    public void Pool(GameEntityType type, MonoBehaviour obj)
    {
        obj.gameObject.SetActive(false);
        if (!_pool.ContainsKey(type))
        {
            _pool.Add(type, new Stack<MonoBehaviour>());
        }

        _pool[type].Push(obj);
    }

    public void IncreasePool(GameEntityType type, MonoBehaviour prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var newObj = Instantiate(prefab);
            Pool(type, newObj);
        }
    }

    public MonoBehaviour Get(GameEntityType type)
    {
        if (_pool.ContainsKey(type) && _pool[type].Count > 0)
        {
            var obj = _pool[type].Pop();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            if (_prefabs.ContainsKey(type))
            {
                Debug.Log($"Adding new objects of type {type} to pool");
                IncreasePool(type, _prefabs[type], 1);
                var obj = _pool[type].Pop();
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                Debug.Log($"Cannot add objects of type {type} to pool");
                return null;
            }
        }
    }
}
