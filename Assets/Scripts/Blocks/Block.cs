using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BlockSFX))]
public class Block : APooledObject<Block>
{
    private BlockSFX _blockSFX;

    public Vector2Int Position {  get; private set; }
    public int Digit { get; private set; }
    public bool IsOne { get; private set; }
    public bool IsBomb { get; private set; }

    public event Action<Block> EventEndMoveDown;

    private float _target;

    private const string NAME = "Block_{0}";

    private void Awake()
    {
        _blockSFX = GetComponent<BlockSFX>();
    }

    private void Setup(Vector2 position)
    {
        _thisTransform.localPosition = position;
        Position = position.ToVector2Int();

        Activate();
    }

    public void SetupDigitis(Vector2 position, BlockSettings settings)
    {
        TypeDigitisSetup(settings);
        Setup(position);
    }

    public void TypeDigitisSetup(BlockSettings settings)
    {
        Digit = settings.Digit;
        IsBomb = Digit == 0;
        IsOne = Digit == 1;

        gameObject.name = string.Format(NAME, Digit);

        if(IsBomb)
            _blockSFX.SetupDigitisBomb(settings);
        else
            _blockSFX.SetupDigitisBlock(settings);
    }

    public void SetupTetris(Vector2 position, Color color, Sprite sprite, Material particleMaterial)
    {
        gameObject.name = string.Format(NAME, gameObject.GetInstanceID());

        _blockSFX.SetupTetris(color, sprite, particleMaterial);
        Setup(position);
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
        _blockSFX.SetTrailEmissionTimeMultiplier(speed);

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
}


