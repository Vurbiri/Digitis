using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BlockVisual))]
public class Block : APooledObject<Block>
{
    public Vector2Int Position {  get; private set; }
    public int Digit { get; private set; }
    

    public event Action<Block> EventEndMoveDown;

    private float _target;
    private const string NAME = "Block_{0}";

    public void Setup(Vector2 position, BlockSettings settings)
    {
        Digit = settings.Digit;
        gameObject.name = string.Format(NAME, Digit);

        _thisTransform.localPosition = position;
        Position = position.ToVector2Int();

        GetComponent<BlockVisual>().Setup(settings);
        Activate();
    }

    public void Transfer(Vector2Int position, Transform parent)
    {
        SetParent(parent);
        _thisTransform.localPosition = position.ToVector3();
        Position = _thisTransform.localPosition.ToVector2Int();
    }
    public void MoveToDelta(Vector2Int delta)
    {
        _thisTransform.localPosition += delta.ToVector3();
        Position += delta;
        _target += delta.y;
    }

    public void MoveDown(float speed, int distance = 1)
    {
        StartCoroutine(MoveDownCoroutine());

        IEnumerator MoveDownCoroutine()
        {
            float y;
            Vector3 position = _thisTransform.localPosition;
            position.y -= distance;
            _target = position.y;
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
}
