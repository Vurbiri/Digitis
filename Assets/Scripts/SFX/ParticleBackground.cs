using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBackground : AParticleSystemController
{
    [SerializeField] private CameraSize _cameraSize;
    [SerializeField] private float _lifePerHeight = 3.4f;
    [SerializeField] private float _emissionPerRadius = 0.25f;

    private void Start()
    {
        RectTransform thisRectTransform = GetComponent<RectTransform>();
        Vector3 positionShape = _shapeModule.position;

        _cameraSize.EventChangingSize += SetupParticleSystem;


        #region Local Functions
        void SetupParticleSystem(Vector2 halfSize)
        {
            Clear();
            Stop();

            positionShape.y = halfSize.y;
            _shapeModule.position = positionShape;
            _shapeModule.radius = halfSize.x;

            _mainModule.startLifetimeMultiplier = halfSize.y * _lifePerHeight;
            _mainModule.duration = _mainModule.startLifetimeMultiplier * 10f;

            _emissionModule.rateOverTimeMultiplier = halfSize.x * _emissionPerRadius;

            Play();
        };
        #endregion
    }
}
