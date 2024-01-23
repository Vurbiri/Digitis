using UnityEngine;

public class Banners : Singleton<Banners>
{
    [SerializeField] private Banner _prefab;
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _repository;
    [SerializeField] int _sizePool = 3;

    private PoolBanners _banners;

    private void Start()
    {
        _banners = new(_prefab, _repository, _sizePool);
    }

    public void Message(string message, MessageType messageType, int time, bool isThrough)
    {
        _banners.GetObject().Setup(_container, message, messageType, time, isThrough);
    }


    public void Clear()
    {
        Transform child;
        while (_container.childCount > 0) 
        {
            child = _container.GetChild(0);
            child.GetComponent<Banner>().Deactivate();
            child.SetParent(_repository);
        }
    }
}
