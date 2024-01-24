
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using NaughtyAttributes;
#endif

public class GameArea : MonoBehaviour
{
    [SerializeField] private Vector2Int _sizeArea = new(10, 20);
    [SerializeField] private BlockSettings[] _blockSettings;
    [Space]
    [SerializeField] private Block _prefabBlock;
    [SerializeField] private Transform _repository;


    private Transform _thisTransform;

    private Pool<Block> _poolBlocks;
    private BlocksArea _area = null;
    private BlocksShape _blocksShape;

    private Coroutine _coroutine;

    private void Awake()
    {
        _thisTransform = transform;
        _poolBlocks = new(_prefabBlock, _repository, 100);
        _area = new(_sizeArea);
        _blocksShape = new(_blockSettings, _area);
        _blocksShape.EventEndMoveDown += OnBlockEndMoveDown;
    }

    private void Start()
    {
        StartTest();
    }


    [Button]
    private void StartTest()
    {
        float speed = 2.5f;
        _blocksShape.Spawn(_poolBlocks.GetObjects(_thisTransform, 2), speed, Random.Range(0, 9) - 4);

    }

    private void OnBlockEndMoveDown(Block block, bool isEnd)
    {
        float speed = 2.5f;

        _area.Add(block);

        if (isEnd)
            _blocksShape.Spawn(_poolBlocks.GetObjects(_thisTransform, 2), speed, Random.Range(0, 9) - 4);
        else
            _blocksShape.BlocksPause(false);
    }

    [Button]
    private void StopTest()
    {
        if( _coroutine != null )
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }


}
