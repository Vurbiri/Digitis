using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BlockSFX))]
public class Block : APooledObject<Block>, IComparable<Block>
{
    private BlockSFX _blockSFX;

    public Vector2Int Position {  get; private set; }
    public int Digit { get; private set; }
    public bool IsOne => Digit == 1;

    public event Action<Block> EventEndMoveDown;

    private float _target;
    private const string NAME = "Block_{0}";

    private void Awake()
    {
        _blockSFX = GetComponent<BlockSFX>();
    }

    public void Setup(Vector2 position, BlockSettings settings)
    {
        Digit = settings.Digit;
        gameObject.name = string.Format(NAME, Digit);

        _thisTransform.localPosition = position;
        Position = position.ToVector2Int();

        _blockSFX.Setup(settings);
        Activate();
    }

    public void ReSetup(BlockSettings settings)
    {
        Digit = settings.Digit;
        gameObject.name = string.Format(NAME, Digit);

        _blockSFX.Setup(settings);
    }

    public bool IsEqual(Block other)
    {
        if (other == null) return false;
        if (this == other) return false;
        return Digit == other.Digit;
    }

    public void Transfer(Vector2Int position, Transform parent)
    {
        _blockSFX.TrailStop();
        SetParent(parent);
        _thisTransform.localPosition = position.ToVector3();
        Position = _thisTransform.localPosition.ToVector2Int();
        _blockSFX.TrailPlay();
    }

    public void Fixed()
    {
        _blockSFX.SetTrailDistanceMultiplier(0f);
    }

    public void MoveToDelta(Vector2Int delta)
    {
        _thisTransform.localPosition += delta.ToVector3();
        Position += delta;
        _target = Position.y;
    }

    public void MoveDown(float speed)
    {
        _blockSFX.SetTrailDistanceMultiplier(speed);

        StartCoroutine(MoveDownCoroutine());

        IEnumerator MoveDownCoroutine()
        {
            float y;
            Vector3 position = _thisTransform.localPosition;
            _target = --position.y;
            speed = Mathf.Abs(speed);

            Position = position.ToVector2Int();

            do
            {
                yield return null;
            } 
            while (!Move(speed * Time.deltaTime));

            EventEndMoveDown?.Invoke(this);

            bool Move(float maxDistanceDelta)
            {
                position = _thisTransform.localPosition;
                y = position.y - maxDistanceDelta;
                if(y > _target)
                {
                    position.y = y;
                    _thisTransform.localPosition = position;
                    return false;
                }
                else
                {
                    position.y = _target;
                    _thisTransform.localPosition = position;
                    return true;
                }
            }
        }
    }

    public async UniTask Remove()
    {
        await _blockSFX.BlockOff();

        base.Deactivate();
    }

    public int CompareTo(Block other)
    {
        return Digit.CompareTo(other.Digit);
    }
}
