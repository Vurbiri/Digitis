using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : APooledObject<T>
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
        T poolObject;
        if (_pool.Count == 0)
            poolObject = CreateObject();
        else
            poolObject = _pool.Pop();

        poolObject.SetParent(parent);
        
        return poolObject;
    }

    public List<T> GetObjects(Transform parent, int count)
    {
        List<T> gameObjects = new(count);
        for (int i = 0; i < count; i ++)
            gameObjects.Add(GetObject(parent));

        return gameObjects;
    }

    private void OnDeactivate(T poolObject)
    {
        poolObject.SetParent(_repository);
        _pool.Push(poolObject as T);
    }

    private T CreateObject()
    {
        T gameObject = Object.Instantiate(_prefab);
        gameObject.Initialize();
        gameObject.EventDeactivate += OnDeactivate;
        return gameObject;
    }
}
