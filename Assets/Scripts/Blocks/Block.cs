using System;
using System.Collections;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

[RequireComponent(typeof(BlockVisual))]
public class Block : APooledObject<Block>
{
    public Vector2Int Index {  get; private set; }
    public int Digit { get; private set; }
    
    public event Action<Block> EventEndMoveDown;

    private float _target;
    private bool _isMove = false;
    private WaitWhile waitNotMove;

    private const string NAME = "Block_{0}";
    
    public void Setup(Vector2 position, BlockSettings settings)
    {
        _thisTransform.localPosition = position;
        Digit = settings.Digit;
        Index = position.ToVector2Int();
        gameObject.name = string.Format(NAME, Digit);
        GetComponent<BlockVisual>().Setup(settings);
        waitNotMove = new(() => _isMove);
        Activate();
    }

    public void Transfer(Vector2Int position, Transform parent)
    {
        SetParent(parent);
        _thisTransform.localPosition = position.ToVector3();
        Index = _thisTransform.localPosition.ToVector2Int();
    }
    public void MoveToDelta(Vector2Int delta)
    {
        StartCoroutine(MoveToDeltaCoroutine());

        IEnumerator MoveToDeltaCoroutine()
        {
            yield return waitNotMove; 
            _thisTransform.localPosition += delta.ToVector3();
            _target += delta.y;
        }
    }

    public void MoveDown(float speed, int distance = 1)
    {
        StartCoroutine(MoveDownCoroutine());

        IEnumerator MoveDownCoroutine()
        {
            float y;
            speed = Mathf.Abs(speed);
            Vector3 position = _thisTransform.localPosition;
            position.y -= distance;
            _target = position.y;

            while (!Move(speed * Time.deltaTime))
                yield return null;

            Index = position.ToVector2Int();
            EventEndMoveDown?.Invoke(this);

            bool Move(float maxDistanceDelta)
            {
                _isMove = true;
                position = _thisTransform.localPosition;
                y = position.y - maxDistanceDelta;
                if(y > _target)
                {
                    position.y = y;
                    _thisTransform.localPosition = position;
                    _isMove = false;
                    return false;
                }
                else
                {
                    position.y = _target;
                    _thisTransform.localPosition = position;
                    _isMove = false;
                    return true;
                }
                
            }
        }
    }
}
