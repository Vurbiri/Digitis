using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour
{
    [SerializeField] private float _verticalSizeMin = 12f;
    [SerializeField] private float _horizontalSizeMin = 7.875f;
    [Space]
    [Space]
    [SerializeField] private float _timeRateUpdate = 1.5f;

    private void Start()
    {
        Camera thisCamera = GetComponent<Camera>();

        StartCoroutine(RatioUpdate());

        IEnumerator RatioUpdate()
        {
            float aspectRatio;
            float aspectRatioOld = 0f;
            float verticalSize;
            WaitForSecondsRealtime delay = new(_timeRateUpdate);

            while (true)
            {
                aspectRatio = thisCamera.aspect;

                if (aspectRatio != aspectRatioOld)
                {
                    aspectRatioOld = aspectRatio;

                    verticalSize = _horizontalSizeMin / aspectRatio;

                    if (verticalSize < _verticalSizeMin)
                        verticalSize = _verticalSizeMin;

                    thisCamera.orthographicSize = verticalSize;
                }

                yield return delay;
            }
        }
    }
}
