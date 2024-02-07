using System;
using System.Collections;
using UnityEngine;

public class CameraSize : MonoBehaviour
{
    [SerializeField] private float _verticalSizeMin = 12.5f;
    [SerializeField] private float _horizontalSizeMin = 8.25f;
    [Space]
    [SerializeField] private float _timeRateUpdateMin = 0.5f;
    [SerializeField] private float _timeRateUpdateMax = 5;
    [SerializeField] private float _timeRateUpdateSteep = 0.25f;
    
    public event Action<float> EventChangingOffsetSizeX;
    public event Action<Vector2> EventChangingSize;

    private void Start()
    {
        Camera thisCamera = GetComponent<Camera>();

        StartCoroutine(RatioUpdate());

        IEnumerator RatioUpdate()
        {
            float timeRateUpdate = _timeRateUpdateMin;
            float aspectRatio;
            float aspectRatioOld = 0f;
            float verticalSize;
            float horizontalSize;
            WaitForSecondsRealtime delay = new(timeRateUpdate);

            while (true)
            {
                yield return delay;

                aspectRatio = thisCamera.aspect;

                if (aspectRatio != aspectRatioOld)
                {
                    aspectRatioOld = aspectRatio;
                    verticalSize = _horizontalSizeMin / aspectRatio;

                    if (verticalSize < _verticalSizeMin)
                        verticalSize = _verticalSizeMin;

                    horizontalSize = verticalSize * aspectRatio;
                    thisCamera.orthographicSize = verticalSize;

                    timeRateUpdate = _timeRateUpdateMin;

                    EventChangingOffsetSizeX?.Invoke(horizontalSize - _horizontalSizeMin);
                    EventChangingSize?.Invoke(new(horizontalSize, verticalSize));
                }
                else
                {
                    if (timeRateUpdate < _timeRateUpdateMax)
                        timeRateUpdate += _timeRateUpdateSteep;
                }

                delay = new(timeRateUpdate);
            }
        }
    }
}
