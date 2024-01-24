#if UNITY_EDITOR
using NaughtyAttributes;
using System.Collections;

#endif

using UnityEngine;

public class GameArea : MonoBehaviour
{
    [SerializeField] private Vector2Int _sizeArea = new(10, 20);
    [SerializeField] private BlockSettings _blockSettings;
    [Space]
    [SerializeField] private Block _prefabBlock;
    [SerializeField] private Transform _repository;

    private int MaxX => _sizeArea.x - 1;
    private int MaxY => _sizeArea.y - 1;

    private Transform _thisTransform;

    private Pool<Block> _poolBlocks;
    private BlocksArea _area = null;

    private Coroutine _coroutine;

    private void Awake()
    {
        _thisTransform = transform;
        _poolBlocks = new(_prefabBlock, _repository, 5);
        _area = new(_sizeArea);
    }


    [Button]
    private void StartTest()
    {
        float speed = 1.75f;
        WaitForSeconds pause = new(1f);
        Vector2Int vector;
        Block block;

        StopTest();
        _coroutine = StartCoroutine(Test());

        IEnumerator Test()
        {
            while (true)
            {
                yield return pause;
                vector = new Vector2Int(Random.Range(0, _sizeArea.x), MaxY);
                if (_area[vector] == null)
                {
                    block = _poolBlocks.GetObject(_thisTransform);
                    block.Setup(vector, _blockSettings);
                    block.EventEndMoveDown += OnEventEndMoveDown;
                    if (_area.IsEmptyDownstairs(vector))
                        block.MoveDown(speed);
                    else
                        _area.Add(block);
                }
            }
        }


        void OnEventEndMoveDown(Block b)
        {
            if (_area.IsEmptyDownstairs(b.Index))
                b.MoveDown(speed);
            else
                _area.Add(b);
        }
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
