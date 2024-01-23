using System.Collections.Generic;
using UnityEngine;

public class PoolBanners
{
    protected readonly Stack<PooledObject> _pool = new();
    private readonly PooledObject _prefab;
    protected readonly Transform _container;

    public PoolBanners(PooledObject prefab, Transform container, int size = 0)
    {
        _prefab = prefab;
        _container = container;
        for (int i = 0; i < size; i++)
            CreateObject().Deactivate();
    }

    public Banner GetObject()
    {
        if (!_pool.TryPop(out PooledObject gameObject))
            gameObject = CreateObject();

        return (gameObject as Banner);
    }

    private void OnDeactivate(PooledObject gameObject) => _pool.Push(gameObject);

    private PooledObject CreateObject()
    {
        PooledObject gameObject;
        gameObject = GameObject.Instantiate<PooledObject>(_prefab, _container);
        gameObject.EventDeactivate += OnDeactivate;

        return gameObject;
    }
}
