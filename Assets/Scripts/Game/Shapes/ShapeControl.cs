using System;
using System.Collections.Generic;
using UnityEngine;

public class ShapeControl
{
    public event Action EventEndMoveDown;
    public event Action EventFixedBlocks;

    private List<Block> _blocks;
    private readonly Queue<Block> _blocksCollision = new();

    private SubShape _shape;
    private Vector2Int _offset;

    private float _speed;
    private bool _isInfinityMode = false;
    private bool _isFixed = true;
    private bool _isSpeedDown = false;
    private float _countBlockMove;

    private readonly Vector2Int startPosition;
    private readonly BlocksArea area;
    private readonly Speeds speeds;
    private readonly Transform container;

    public ShapeControl(BlocksArea area, Speeds speeds, bool isInfinityMode)
    {
        this.area = area;
        this.speeds = speeds;
        _isInfinityMode = isInfinityMode;
        container = area.Container;
        startPosition = new(area.Size.x / 2 - 1, area.Size.y);
    }

    public bool SetupForNew(Shape shapeForm)
    {
        _isFixed = true;


        CopyFromShapeForm();

        bool isEmpty = true;
        Vector2Int position;
        Block block;

        for(int i = 0; i < _blocks.Count; i++)
        {
            block = _blocks[i];
            position = shapeForm.BlocksPositions[i] + _offset;
            block.Transfer(position, container);
            block.StartFall(speeds.Current);
            isEmpty = isEmpty && area.IsEmptyDownstairs(position);
            block.EventEndMoveDown += OnEventEndMoveDown;
        }

        ResetParameters();

        return isEmpty;

        #region Local Functions
        void CopyFromShapeForm()
        {
            _shape = shapeForm.SubShape;
            _blocks = shapeForm.Blocks;
            _offset = startPosition + (_isInfinityMode ? shapeForm.StartOffsetInfinity : shapeForm.StartOffset);
        }
        void ResetParameters()
        {
            if (!isEmpty) return;

            _speed = speeds.Current;
            _countBlockMove = _blocks.Count;
            _isFixed = false;
            _isSpeedDown = false;
            Debug.Log(_isFixed);
        }
        #endregion
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

    public void SetSpeed(bool isSpeedDown)
    {
        if (_isFixed)
            return;

        _isSpeedDown = isSpeedDown;
        _speed = isSpeedDown ? speeds.Down : speeds.Current;
        if (_isInfinityMode)
            _blocks.ForEach(block => block.MoveDown(_speed));
        else
            _blocks.ForEach(block => block.Speed = _speed);
    }

    public void StartMoveDown()
    {
        if (_speed == 0)
            return;

        _offset.y -= 1;
        _blocks.ForEach(block => block.MoveDown(_speed));
    }

    public bool TryShift(Vector2Int direct)
    {
        if (_isFixed || _isSpeedDown)
            return false; 

        if (!area.IsEmptyArea(_blocks, direct))
            return false;

        _offset += direct;
        for (int i = 0; i < _blocks.Count; i++)
            _blocks[i].MoveToDelta(direct);

        return true;
    }
    
    public bool TryRotate()
    {
        if(_isFixed || _isSpeedDown)
            return false;

        if (!area.IsEmptyArea(_shape.CollisionRotation, _offset))
            return false;

        for (int i = 0; i < _blocks.Count; i++)
            _blocks[i].MoveToDelta(_shape.DeltaPositions[i]);

        _shape = _shape.NextSubShape; 

        return true;
    }

    private void OnEventEndMoveDown(Block block)
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
