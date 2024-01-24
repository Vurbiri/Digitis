using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BlockVisual))]
public class Block : PooledObject
{
    public Vector2Int Index {  get; private set; }
    public int Digit { get; private set; }
    public bool isMovePause { get; set; }

    public event Action<Block> EventEndMoveDown;
    
    public void Setup(Vector2 position, BlockSettings settings)
    {
        _thisTransform.localPosition = position;
        Index = position.ToVector2Int();
        Digit = settings.Digit;
        GetComponent<BlockVisual>().Setup(settings);
        Activate();
        isMovePause = false;
    }

    public void MoveDown(float speed)
    {
        StartCoroutine(MoveDownCoroutine());

        IEnumerator MoveDownCoroutine()
        {
            float y;
            speed = Mathf.Abs(speed);
            Vector3 position = _thisTransform.localPosition;
            position.y -= 1f;
            float target = position.y;

            Index = position.ToVector2Int();

            while (!Move(speed * Time.deltaTime))
            {
                yield return null;
            }

            EventEndMoveDown?.Invoke(this);

            bool Move(float maxDistanceDelta)
            {
                if(isMovePause)
                    return false;
                
                position = _thisTransform.localPosition;
                y = position.y - maxDistanceDelta;
                if(y > target)
                {
                    position.y = y;
                    _thisTransform.localPosition = position;
                    return false;
                }
                else
                {
                    position.y = target;
                    _thisTransform.localPosition = position;
                    return true;
                }
            }
        }
    }
}
