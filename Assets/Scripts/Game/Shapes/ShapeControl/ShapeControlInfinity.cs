using UnityEngine;

public class ShapeControlInfinity : AShapeControl
{
    public ShapeControlInfinity(BlocksArea area, Speeds speeds) : base(area, speeds)
    {
    }

    public override bool SetupForNew(Shape shapeForm)
    {
        if (!area.IsEmptyStartArea)
            return false;

        CopyFromShapeForm();

        Block block;

        for (int i = 0; i < _blocks.Count; i++)
        {
            block = _blocks[i];
            block.Transfer(shapeForm.BlocksPositions[i] + _offset, container);
            block.StartFall(speeds.Current);
            block.EventEndMoveDown += OnEventEndMoveDown;
        }
        ResetParameters();

        return true;

        #region Local Functions
        void CopyFromShapeForm()
        {
            _shape = shapeForm.SubShape;
            _blocks = shapeForm.Blocks;
            _offset = startPosition + shapeForm.StartOffsetInfinity;
        }
        #endregion
    }

    public override bool SetSpeed(bool isSpeedDown)
    {
        if (_isFixed || _isSpeedDown || !isSpeedDown)
            return false;

        _isSpeedDown = isSpeedDown;
        _speed = speeds.Down;
        for (int i = 0; i < _blocks.Count; i++)
            OnEventEndMoveDown(_blocks[i]);
        return true;
    }

    public override bool TryShift(Vector2Int direct)
    {
        if (_isFixed || _isSpeedDown)
            return false;

        if (!area.IsCorrectArea(_blocks, direct.x))
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

        if (!area.IsCorrectArea(_shape.CollisionRotation, _offset.x))
            return false;

        for (int i = 0; i < _blocks.Count; i++)
            _blocks[i].MoveToDelta(_shape.DeltaPositions[i]);

        _shape = _shape.NextSubShape;

        return true;
    }
}
