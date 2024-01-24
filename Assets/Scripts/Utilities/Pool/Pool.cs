using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : PooledObject
{
    private readonly Stack<T> _pool = new();
    private readonly T _prefab;
    private readonly Transform _repository;

    public Pool(T prefab, Transform repository, int size = 0)
    {
        _prefab = prefab;
        _repository = repository;
        for (int i = 0; i < size; i++)
            OnDeactivate(CreateObject());
    }

    public T GetObject(Transform parent)
    {
        T gameObject;
        if (_pool.Count == 0)
            gameObject = CreateObject();
        else
            gameObject = _pool.Pop();

        gameObject.SetParent(parent);
        
        return gameObject;
    }

    private void OnDeactivate(PooledObject gameObject)
    {
        gameObject.SetParent(_repository);
        _pool.Push(gameObject as T);
    }

    private T CreateObject()
    {
        T gameObject = Object.Instantiate(_prefab);
        gameObject.Initialize();
        gameObject.EventDeactivate += OnDeactivate;
        return gameObject;
    }
}
