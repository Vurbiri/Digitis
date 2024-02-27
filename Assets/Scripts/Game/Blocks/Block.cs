using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(BlockSFX))]
public class Block : APooledObject<Block>
{
    public Vector2Int Position {  get; private set; }
    public int Digit { get; private set; }
    public bool IsOne { get; private set; }
    public bool IsBomb { get; private set; }
    public float Speed { get => _speed; set { _speed = value; _blockSFX.SetTrailEmissionTimeMultiplier(_speed); } }
    public bool IsDeferredOrder { get; private set; }

    public event Action<Block> EventEndMoveDown;

    private BlockSFX _blockSFX;
    private float _target;
    private float _speed;
    
    private const string NAME = "Block_{0}";

    private void Awake()
    {
        _blockSFX = GetComponent<BlockSFX>();
    }

    public void Setup(Vector2 position, BlockSettings settings)
    {
        TypeSetup(settings);

        _thisTransform.localPosition = position;
        Position = position.ToVector2Int();

        Activate();
    }

    public void TypeSetup(BlockSettings settings)
    {
        Digit = settings.Digit;
        IsBomb = Digit == 0;
        IsOne = Digit == 1;

        gameObject.name = string.Format(NAME, Digit);

        if(IsBomb)
            _blockSFX.SetupBomb(settings);
        else
            _blockSFX.SetupBlock(settings);
    }

    public bool IsEqualDigit(Block other)
    {
        if (other == null) return false;
        if (this == other) return false;
        return Digit == other.Digit;
    }

    public void Transfer(Vector2Int position, Transform parent)
    {
        _blockSFX.Transfer();
        SetParent(parent);
        _thisTransform.localPosition = position.ToVector3();
        Position = _thisTransform.localPosition.ToVector2Int();
        IsDeferredOrder = false;
    }

    public void StartFall(float speed) => _blockSFX.StartFall(speed);
    public void Fixed() => _blockSFX.Fixed();

    public void MoveToDelta(Vector2Int delta)
    {
        _thisTransform.localPosition += delta.ToVector3();
        Position += delta;
        _target = Position.y;
    }

    public void MoveDown(float speed)
    {
        Speed = speed;

        StartCoroutine(MoveDownCoroutine());

        IEnumerator MoveDownCoroutine()
        {
            float y;
            Vector3 position = _thisTransform.localPosition;
            _target = --position.y;
            Position = position.ToVector2Int();

            do
            {
                yield return null;
            } 
            while (!Move());

            EventEndMoveDown?.Invoke(this);

            bool Move()
            {
                position = _thisTransform.localPosition;
                y = position.y - _speed * Time.deltaTime;
                if(y > _target)
                {
                    position.y = y;
                    _thisTransform.localPosition = position;
                    IsDeferredOrder = y - _target < 0.2f;
                    return false;
                }
                else
                {
                    position.y = _target;
                    _thisTransform.localPosition = position;
                    IsDeferredOrder = false;
                    return true;
                }
            }
        }
    }

    public async UniTask Explode()
    {
        if(IsBomb)
            await _blockSFX.ExplodeBomb();
        else
            await _blockSFX.Explode();

        base.Deactivate();
    }

    public async UniTask Remove()
    {
        await _blockSFX.Remove();

        base.Deactivate();
    }

    public async UniTask Remove(float volume, CancellationToken cancellationToken)
    {
        await _blockSFX.Remove(volume, cancellationToken);

        if (cancellationToken.IsCancellationRequested)
            return;

        base.Deactivate();
    }
}


