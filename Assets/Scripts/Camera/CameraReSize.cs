using System;
using UnityEngine;

public class CameraReSize : MonoBehaviour
{
    protected Camera _thisCamera;
    private float aspectRatioOld = 0f;

    private Vector2 _size = Vector2.zero;
    public Vector2 Size => _size;

    public event Action<Vector2> EventReSize;
    
    private void Start()
    {
        _thisCamera = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        if (aspectRatioOld == _thisCamera.aspect)
            return;

        aspectRatioOld = _thisCamera.aspect;
        OnReSize(_thisCamera.orthographicSize * aspectRatioOld, _thisCamera.orthographicSize, aspectRatioOld);
    }

    protected virtual void OnReSize(float horizontalHalfSize, float verticalHalfSize, float aspectRatio)
    {
        _size.x = 2 * horizontalHalfSize; _size.y = 2 * verticalHalfSize;
        EventReSize?.Invoke(new(horizontalHalfSize, verticalHalfSize));
    }
}
