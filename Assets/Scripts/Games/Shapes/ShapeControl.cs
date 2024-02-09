using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShapeControl
{
    public event Action EventEndMoveDown;

    private List<Block> _blocks;
    private readonly Queue<Block> _blocksCollision = new();

    private SubShape _shape;
    private Vector2Int _offset;
    private Vector2Int _deferredShift;
    private bool _deferredRotation;

    private float _speed;
    private bool _isFixed = true;
    private bool _isSpeedDown = false;
    private float _countBlockMove;
    private bool _isCollision = false;
    private bool  _isGravity;

    private int? _moveCount = null;

    private readonly Vector2Int startPosition;
    private readonly BlocksArea area;
    private readonly Speeds speeds;
    private readonly Transform container;

    private const int WAIT_BEFORE_FIXED_LESS_6 = 500;
    private const int WAIT_BEFORE_FIXED = 3000;

    public ShapeControl(BlocksArea area, Speeds speeds)
    {
        this.area = area;
        this.speeds = speeds;
        container = area.Container;
        startPosition = new(area.Size.x / 2 - 1 , area.Size.y);
    }

    public bool SetupForNew(Shape shapeForm, bool isGravity)
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
            block.StartFall(speeds.Current);
            isEmpty = isEmpty && area.IsEmptyDownstairs(position);
            if (isGravity)
                block.EventEndMoveDown += OnEventEndMoveDownGravity;
            else
                block.EventEndMoveDown += OnEventEndMoveDownNotGravity;
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
            _deferredShift = Vector2Int.zero;
            _deferredRotation = false;
            _isFixed = false;
            _isSpeedDown = false;
            _isCollision = false;
            _moveCount = null;
            _countBlockMove = _blocks.Count;
            _isGravity = isGravity;
        }
        #endregion
    }
    public void SetupForFall(List<Block> blocks, bool isGravity, int count)
    {
        _blocks = blocks;
        _speed = speeds.Fall;
        _deferredShift = Vector2Int.zero;
        _deferredRotation = false;
        _isFixed = true;
        _isSpeedDown = false;
        _isCollision = false;
        _countBlockMove = _blocks.Count;
        _moveCount = isGravity ? null : count;
        _isGravity = isGravity;
        foreach (var block in _blocks)
        {
            block.EventEndMoveDown += OnEventEndMoveDownGravity;
            block.StartFall(speeds.Fall);
        }
    }

    public void SetSpeed(bool isSpeedDown)
    {
        if (_isFixed)
            return;

        _isSpeedDown = isSpeedDown;
        _speed = isSpeedDown ? speeds.Down : speeds.Current;
        _blocks.ForEach(block => block.Speed = _speed);
    }

    public void StartMoveDown()
    {
        if(_moveCount != null && _moveCount-- <= 0)
        {
            StopMove(OnEventEndMoveDownGravity);
            return;
        }
        
        _offset.y -= 1;
        _blocks.ForEach(block => block.MoveDown(_speed));
    }

    public void TryShift(Vector2Int direct)
    {
        if (_isFixed || _isSpeedDown || _deferredShift == direct)
            return;

        if (!area.IsEmptyArea(_blocks, direct))
            return;

        if (!_isGravity && !_isCollision)
        {
            if (!area.IsEmptyArea(_blocks, direct + Vector2Int.up))
            {
                if (_blocks[0].IsDeferredOrder)
                {
                    _deferredShift = direct;
                    _deferredRotation = false;
                }
                return;
            }
        }

        Shift(direct);
    }
    private void Shift(Vector2Int direct)
    {
        _deferredShift = Vector2Int.zero;
        _offset += direct;
        for (int i = 0; i < _blocks.Count; i++)
            _blocks[i].MoveToDelta(direct);
    }
    
    public void TryRotate()
    {
        if(_isFixed || _isSpeedDown || _deferredRotation)
            return;

        if (!area.IsEmptyArea(_shape.CollisionRotation, _offset))
            return;

        if (!_isGravity && !_isCollision)
        {
            if (!area.IsEmptyArea(_shape.CollisionRotation, _offset + Vector2Int.up))
            {
                if (_blocks[0].IsDeferredOrder)
                {
                    _deferredShift = Vector2Int.zero;
                    _deferredRotation = true;
                }
                return;
            }
        }

        Rotate();
    }
    private void Rotate()
    {
        for (int i = 0; i < _blocks.Count; i++)
            _blocks[i].MoveToDelta(_shape.DeltaPositions[i]);
       
        _deferredRotation = false;
        _shape = _shape.NextSubShape;
    }

    private void OnEventEndMoveDownGravity(Block block)
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
            FixedBlock(_blocksCollision.Dequeue(), OnEventEndMoveDownGravity);

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
                    FixedBlock(_blocks[count], OnEventEndMoveDownGravity);
                    count = _blocks.Count;
                }
            }
        }
        #endregion
    }
    private void OnEventEndMoveDownNotGravity(Block block)
    {
        if (!area.IsEmptyDownstairs(block))
            _isCollision = true;

        if (--_countBlockMove > 0)
            return;

        if (!_isCollision)
        {
            if(DeferredOrder())
                CheckingBoxes();
            else
                ReStartMoveDown();
        }
        else if (_isSpeedDown)
        {
            _isFixed = true;
            StopMove(OnEventEndMoveDownNotGravity);
        }
        else
        {
            DeferredOrder();
            CheckingBoxesAsync().Forget();
        }

        #region Local Functions
        async UniTaskVoid CheckingBoxesAsync()
        {
            await UniTask.Delay((int)(speeds.Current < 6f ? WAIT_BEFORE_FIXED_LESS_6 : WAIT_BEFORE_FIXED / speeds.Current));

            CheckingBoxes();
        }
        void CheckingBoxes()
        {
            for (int i = _blocks.Count - 1; i >= 0; i--)
            {
                if (!area.IsEmptyDownstairs(_blocks[i]))
                {
                    _isFixed = true;
                    FixedBlock(_blocks[i], OnEventEndMoveDownNotGravity);
                }
            }

            if (!_isFixed)
            {
                ReStartMoveDown();
                return;
            }

            StopMove(OnEventEndMoveDownNotGravity);
        }
        void ReStartMoveDown()
        {
            _isCollision = false;
            _countBlockMove = _blocks.Count;
            StartMoveDown();
        }
        bool DeferredOrder()
        {
            bool isOrder = _deferredRotation;
            if (_deferredRotation)
                Rotate();
            else if (isOrder = _deferredShift != Vector2Int.zero)
                Shift(_deferredShift);
            return isOrder;
        }
        #endregion
    }

    private void StopMove(Action<Block> callback)
    {
        while (_blocks.Count > 0)
            FixedBlock(_blocks[0], callback);

        EventEndMoveDown?.Invoke();
    }

    private void FixedBlock(Block block, Action<Block> callback)
    {
        block.EventEndMoveDown -= callback;
        block.Fixed();
        area.Add(block);
        _blocks.Remove(block);
    }
}
