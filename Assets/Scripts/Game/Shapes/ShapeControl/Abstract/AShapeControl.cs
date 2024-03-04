using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AShapeControl
{
    public event Action EventEndMoveDown;
    public event Action EventFixedBlocks;

    protected List<Block> _blocks;
    protected readonly Queue<Block> _blocksCollision = new();

    protected SubShape _shape;
    protected Vector2Int _offset;

    protected float _speed;

    protected bool _isFixed = true;
    protected bool _isSpeedDown = false;
    protected float _countBlockMove;

    protected readonly Vector2Int startPosition;
    protected readonly BlocksArea area;
    protected readonly Speeds speeds;
    protected readonly Transform container;

    public AShapeControl(BlocksArea area, Speeds speeds)
    {
        this.area = area;
        this.speeds = speeds;
        container = area.Container;
        startPosition = new(area.Size.x / 2 - 1, area.Size.y);
    }

    public abstract bool SetupForNew(Shape shapeForm);
    protected void ResetParameters()
    {
        _speed = speeds.Current;
        _countBlockMove = _blocks.Count;
        _isFixed = false;
        _isSpeedDown = false;
    }

    public void SetupForFall(List<Block> blocks)
    {
        _blocks = blocks;
        _speed = speeds.Fall;
        _isFixed = true;
        _isSpeedDown = false;
        _countBlockMove = _blocks.Count;
        foreach (var block in _blocks)
        {
            block.EventEndMoveDown += OnEventEndMoveDown;
            block.StartFall(speeds.Fall);
        }
    }

    public void StartMoveDown()
    {
        _offset.y -= 1;
        _blocks.ForEach(block => block.MoveDown(_speed));
    }

    public abstract bool SetSpeed(bool isSpeedDown);

    public abstract bool TryShift(Vector2Int direct);
    public abstract bool TryRotate();

    protected void OnEventEndMoveDown(Block block)
    {
        if (!area.IsEmptyDownstairs(block))
        {
            _isFixed = true;
            _blocksCollision.Enqueue(block);
        }

        if (--_countBlockMove > 0)
            return;

        if (_blocksCollision.Count == 0)
        {
            _countBlockMove = _blocks.Count;
            StartMoveDown();
            return;
        }

        _speed = speeds.Fall;

        while (_blocksCollision.Count > 0)
            FixedBlock(_blocksCollision.Dequeue());

        CheckingBoxes();

        if(!block.IsBomb)
            EventFixedBlocks.Invoke();

        _countBlockMove = _blocks.Count;
        if (_countBlockMove == 0)
            EventEndMoveDown?.Invoke();
        else
            StartMoveDown();

        #region Local Functions
        void CheckingBoxes()
        {
            int count = _blocks.Count;
            while (count > 0)
            {
                if (!area.IsEmptyDownstairs(_blocks[--count]))
                {
                    FixedBlock(_blocks[count]);
                    count = _blocks.Count;
                }
            }
        }
        void FixedBlock(Block block)
        {
            block.EventEndMoveDown -= OnEventEndMoveDown;
            block.Fixed();
            area.Add(block);
            _blocks.Remove(block);
        }
        #endregion
    }
}
