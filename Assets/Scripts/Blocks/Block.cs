using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{

    private Transform _thisTransform;

    private void Awake()
    {
        _thisTransform = transform;
        Debug.Log(_thisTransform.localPosition);
        Debug.Log(_thisTransform.position);
    }

    public void MoveTargetY(float target, float speed)
    {
        StartCoroutine(MoveDownCoroutine());

        IEnumerator MoveDownCoroutine()
        {
            Vector3 position;
            float y;
            speed = Mathf.Abs(speed);

            do
            {
                yield return null;
            }
            while (!Move(speed * Time.deltaTime)); 
        

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
