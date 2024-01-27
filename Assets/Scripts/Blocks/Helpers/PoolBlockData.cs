using UnityEngine;

[System.Serializable]
public class PoolBlockData
{
    [SerializeField] private Block _prefabBlock;
    [SerializeField] private int _sizePool = 100;
    [SerializeField] private BlockSettings[] _blockSettings;
    [Space]
    [SerializeField] private Transform _poolRepository;
    [Space]
    [SerializeField] private Transform _gameAreaContainer;
    [SerializeField] private Transform _nextAreaContainer;

    public Block PrefabBlock => _prefabBlock;
    public int SizePool => _sizePool;
    public BlockSettings[] BlockSettings => _blockSettings;
    public Transform PoolRepository => _poolRepository;
    public Transform GameAreaContainer => _gameAreaContainer;
    public Transform NextAreaContainer => _nextAreaContainer;

    public const int MIN_DIGIT = 1;

}
