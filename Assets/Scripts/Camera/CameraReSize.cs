using System;
using System.Collections;
using UnityEngine;

public class CameraReSize : MonoBehaviour
{
    [SerializeField] private float _timeRateUpdateMin = 0.5f;
    [SerializeField] private float _timeRateUpdateMax = 5;
    [SerializeField] private float _timeRateUpdateSteep = 0.25f;

    protected Camera _thisCamera;

    public event Action<Vector2> EventReSize;
    
    private void Start()
    {
        _thisCamera = GetComponent<Camera>();

        StartCoroutine(RatioUpdate());
    }

    private IEnumerator RatioUpdate()
    {
        float timeRateUpdate = _timeRateUpdateMin;
        float aspectRatio;
        float aspectRatioOld = 0f;
        WaitForSecondsRealtime delay = new(timeRateUpdate);

        while (true)
        {
            yield return delay;

            aspectRatio = _thisCamera.aspect;

            if (aspectRatio != aspectRatioOld)
            {
                aspectRatioOld = aspectRatio;
                timeRateUpdate = _timeRateUpdateMin;

                OnReSize(_thisCamera.orthographicSize * aspectRatio, _thisCamera.orthographicSize, aspectRatio);
            }
            else
            {
                if (timeRateUpdate < _timeRateUpdateMax)
                    timeRateUpdate += _timeRateUpdateSteep;
            }

            delay = new(timeRateUpdate);
        }
    }

    protected virtual void OnReSize(float horizontalHalfSize, float verticalHalfSize, float aspectRatio)
    {
        EventReSize?.Invoke(new(horizontalHalfSize, verticalHalfSize));
    }
}
