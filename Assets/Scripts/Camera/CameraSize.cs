using System;
using UnityEngine;

public class CameraSize : CameraReSize
{
    [Space]
    [SerializeField] private float _verticalSizeMin = 12.55f;
    [SerializeField] private float _horizontalSizeMin = 8.25f;
    
    public event Action<float> EventChangingOffsetSizeX;

    protected override void OnReSize(float horizontalHalfSize, float verticalHalfSize, float aspectRatio)
    {
        horizontalHalfSize = _horizontalSizeMin;
        verticalHalfSize = _horizontalSizeMin / aspectRatio;

        if (verticalHalfSize < _verticalSizeMin)
        {
            verticalHalfSize = _verticalSizeMin;
            horizontalHalfSize = verticalHalfSize * aspectRatio;
        }

        _thisCamera.orthographicSize = verticalHalfSize;

        EventChangingOffsetSizeX?.Invoke(horizontalHalfSize - _horizontalSizeMin);
        base.OnReSize(horizontalHalfSize, verticalHalfSize, aspectRatio);
    }
}
