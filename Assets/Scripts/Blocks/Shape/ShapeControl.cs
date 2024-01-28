using System;
using System.Collections.Generic;
using UnityEngine;

public class ShapeControl
{
    public event Action EventEndMoveDown;

    private List<Block> _blocks;
    private readonly Queue<Block> _blocksCollision;

    private SubShape _shape;
    private Vector2Int _offset;

    private float _speed;
    private bool _isFixed = true;
    private float _countBlockMove;
    
    private readonly Vector2Int startPosition;
    private readonly BlocksArea area;
    private readonly Speeds speeds;
    private readonly Transform container;

    private const int WAIT_BEFORE_FIXED = 250;
    private const int MAX_COUNT_BLOCK = 4;

    public ShapeControl(BlocksArea area, Speeds speeds)
    {
        _blocksCollision = new(MAX_COUNT_BLOCK);

        this.area = area;
        this.speeds = speeds;
        container = area.Container;
        startPosition = new(area.Size.x / 2 , area.Size.y);
    }

    public bool Create(Shape shapeForm)
    {
        CopyFromShapeForm();

        bool isEmpty = true;
        Vector2Int position;
        Block block;

        for(int i = 0; i < _blocks.Count; i++)
        {
            block = _blocks[i];
            position = shapeForm.BlocksPositions[i] + _offset;
            block.Transfer(position, container);
            block.EventEndMoveDown += OnEventEndMoveDown;
            isEmpty = isEmpty && area.IsEmptyDownstairs(position);
        }

        ResetParameters();

        return isEmpty;

        #region Local Functions
        void CopyFromShapeForm()
        {
            _shape = shapeForm.SubShape;
            _blocks = shapeForm.Blocks;
            _offset = startPosition + shapeForm.StartOffset;
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
        foreach (var block in _blocks)
            block.MoveDown(_speed);
    }

    public void Shift(Vector2Int direct)
    {
        if (_isFixed)
            return;

        if (!area.IsEmptyArea(_blocks, direct))
            return;

        _offset += direct;
        for (int i = 0; i < _blocks.Count; i++)
            _blocks[i].MoveToDelta(direct);
    }
    
    public void Rotate()
    {
        if(_isFixed)
            return;

        if (!area.IsEmptyArea(_shape.CollisionRotation, _offset))
            return;
        
        for (int i = 0; i < _blocks.Count; i++)
            _blocks[i].MoveToDelta(_shape.DeltaPositions[i]);

        _shape = _shape.NextSubShape;
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

        if(_blocksCollision.Count == 0)
        {
            _countBlockMove = _blocks.Count;
            StartMoveDown();
            return;
        }

        _speed = speeds.Fall;

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
            area.Add(block);
            _blocks.Remove(block);
        }
        #endregion
    }
    
}
