using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShapeControl
{
    public event Action EventEndMoveDown;

    private Dictionary<Vector2Int, Block> _blocks;

    private Vector2Int _offset;
    private Vector2Int _maxIndexes;

    private float _speed;
    private bool _isFixed = true;
    private float _countBlockMove;
    
    private readonly Queue<Block> _blocksCollision;

    private readonly Vector2Int startPosition;
    private readonly BlocksArea area;
    private readonly Speeds speeds;
    private readonly Transform container;

    private const int WAIT_BEFORE_FIXED = 250;
    private const int MAX_COUNT_BLOCK = 4;

    public ShapeControl(BlocksArea area, Speeds speeds, Transform container)
    {
        _blocks = new(MAX_COUNT_BLOCK);
        _blocksCollision = new(MAX_COUNT_BLOCK);

        this.area = area;
        this.speeds = speeds;
        this.container = container;
        startPosition = new(area.Size.x / 2 , area.Size.y);
    }

    public bool Create(AShapeForm shapeForm)
    {
        CopyFromShapeForm();

        bool isEmpty = true;
        Vector2Int position;

        foreach (var (key, block) in _blocks)
        {
            position = key + _offset;
            block.Transfer(position, container);
            block.EventEndMoveDown += OnEventEndMoveDown;
            isEmpty = isEmpty && area.IsEmptyDownstairs(position);
        }

        ResetParameters();

        return isEmpty;

        #region Local Functions
        void CopyFromShapeForm()
        {
            _blocks = shapeForm.Blocks;
            _offset = startPosition;
            _offset.x -= shapeForm.Size.x / 2;
            _offset.x += UnityEngine.Random.Range(0, 9) - 4;
            _maxIndexes = shapeForm.Size - Vector2Int.one;
        }
        void ResetParameters()
        {
            if (!isEmpty) return;

            _speed = speeds.Current;
            _isFixed = false;
            _countBlockMove = _blocks.Count;
        }
        #endregion
    }

    public void StartMoveDown()
    {
        _offset.y -= 1;
        foreach (var block in _blocks.Values)
            block.MoveDown(_speed);
    }
    
    public bool Rotate()
    {
        if(_isFixed)
            return false;

        int count = _blocks.Count;
        Dictionary<Vector2Int, Vector2Int> indexes = new(count);
        Vector2Int temp;
        foreach (var key in _blocks.Keys)
        {
            temp = new(_maxIndexes.y - key.y, key.x);
            if (!area.IsEmptyCell(temp + _offset))
                return false;
            indexes[key] = temp;
        }

        Dictionary<Vector2Int, Block> newBlocks = new(count);
        Block block;

        foreach (var (oldIndex, newIndex) in indexes)
        {
            block = _blocks[oldIndex];
            newBlocks[newIndex] = block;
            block.MoveToDelta(newIndex - oldIndex);
        }
        _blocks = newBlocks;
        return true;
    }

    private void OnEventEndMoveDown(Block block)
    {
        if (!area.IsEmptyDownstairs(block))
            _blocksCollision.Enqueue(block);

        if (--_countBlockMove > 0)
            return;

        if(_blocksCollision.Count == 0)
        {
            _countBlockMove = _blocks.Count;
            StartMoveDown();
            return;
        }

        while (_blocksCollision.Count > 0)
            FixedBlock(_blocksCollision.Dequeue());

        CheckingBoxes();

        _countBlockMove = _blocks.Count;
        if (_countBlockMove == 0)
            EventEndMoveDown?.Invoke();
        else
            StartMoveDown();

        #region Local Functions
        void CheckingBoxes()
        {
            Block block = default;
            Vector2Int index = default;
            bool isEnd = true;

            while (_blocks.Count > 0)
            {
                isEnd = true;
                foreach (var (k,b) in _blocks)
                {
                    if (!area.IsEmptyDownstairs(b))
                    {
                        block = b;
                        index = k;
                        isEnd = false;
                        break;
                    }
                }
                if (isEnd) return;

                FixedKeyBlock(index, block);
            }
        }
        void FixedKeyBlock(Vector2Int key, Block block)
        {
            _isFixed = true;
            _speed = speeds.Fall;

            area.Add(block);
            block.EventEndMoveDown -= OnEventEndMoveDown;
            _blocks.Remove(key);
        }
        void FixedBlock(Block block) => FixedKeyBlock(block.Index - _offset, block);
        #endregion
    }
    
}
