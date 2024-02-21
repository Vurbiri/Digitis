using UnityEngine;

public abstract class AParticleBackgroundMobile : AParticleBackground
{
    private Vector3 _positionShape;

    protected override void Awake()
    {
        base.Awake();
        _mainModule.duration = _mainModule.startLifetimeMultiplier * _timeRecolor;
        Play();

        _positionShape = _shapeModule.position;
    }

    protected override void OnReSizeParticleSystem(Vector2 halfSize)
    {
        ClearAndStop();

        _positionShape.y = halfSize.y;
        _shapeModule.position = _positionShape;
        _shapeModule.radius = halfSize.x;

        _mainModule.startLifetimeMultiplier = Mathf.Ceil(2f * halfSize.y / _mainModule.startSpeedMultiplier);
        _mainModule.duration = _mainModule.startLifetimeMultiplier * _timeRecolor;

        _emissionModule.rateOverTimeMultiplier = halfSize.x * _emissionPerRadius;

        Play();
    }
}
