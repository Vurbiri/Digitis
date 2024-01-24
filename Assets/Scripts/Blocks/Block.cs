using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BlockVisual))]
public class Block : PooledObject
{
    public Vector2Int Index {  get; private set; }
    
    public event Action<Block> EventEndMoveDown;
    
    public void Setup(Vector2 position, BlockSettings settings)
    {
        _thisTransform.localPosition = position;
        Index = position.ToVector2Int();
        GetComponent<BlockVisual>().Setup(settings);
        Activate();
    }

    public void MoveDown(float speed)
    {
        StartCoroutine(MoveDownCoroutine());

        IEnumerator MoveDownCoroutine()
        {
            float target = _thisTransform.localPosition.y - 1f;
            Vector3 position;
            float y;
            speed = Mathf.Abs(speed);

            do
            {
                yield return null;
            }
            while (!Move(speed * Time.deltaTime));

            Index = position.ToVector2Int();
            EventEndMoveDown?.Invoke(this);

            bool Move(float maxDistanceDelta)
            {
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
