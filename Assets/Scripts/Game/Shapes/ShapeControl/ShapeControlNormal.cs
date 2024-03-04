using UnityEngine;

public class ShapeControlNormal : AShapeControl
{
    public ShapeControlNormal(BlocksArea area, Speeds speeds) : base(area, speeds)
    {
    }

    public override bool SetupForNew(Shape shapeForm)
    {
        CopyFromShapeForm();

        bool isEmpty = true;
        Vector2Int position;
        Block block;

        for (int i = 0; i < _blocks.Count; i++)
        {
            block = _blocks[i];
            position = shapeForm.BlocksPositions[i] + _offset;
            block.Transfer(position, container);
            block.StartFall(speeds.Current);
            isEmpty = isEmpty && area.IsEmptyDownstairs(position);
            block.EventEndMoveDown += OnEventEndMoveDown;
        }

        if (isEmpty)
            ResetParameters();

        return isEmpty;

        #region Local Functions
        void CopyFromShapeForm()
        {
            _shape = shapeForm.SubShape;
            _blocks = shapeForm.Blocks;
            _offset = startPosition + shapeForm.StartOffset;
        }
        #endregion
    }


    public override bool SetSpeed(bool isSpeedDown)
    {
        if (_isFixed)
            return false;

        _isSpeedDown = isSpeedDown;
        _speed = isSpeedDown ? speeds.Down : speeds.Current;
        _blocks.ForEach(block => block.Speed = _speed);
        return true;
    }

    public override bool TryShift(Vector2Int direct)
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

    public override bool TryRotate()
    {
        if (_isFixed || _isSpeedDown)
            return false;

        if (!area.IsEmptyArea(_shape.CollisionRotation, _offset))
            return false;

        for (int i = 0; i < _blocks.Count; i++)
            _blocks[i].MoveToDelta(_shape.DeltaPositions[i]);

        _shape = _shape.NextSubShape;

        return true;
    }

}
