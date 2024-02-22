using UnityEngine;

public class ParticleBackgroundMenuMobile : AParticleBackground
{
    private Vector3 _positionShape;

    protected override void Awake()
    {
        base.Awake();

        _positionShape = _shapeModule.position;
    }

    protected override void OnReSizeParticleSystem(Vector2 halfSize)
    {
        ClearAndStop();

        _positionShape.y = halfSize.y;
        _shapeModule.position = _positionShape;
        _shapeModule.radius = halfSize.x;

        _mainModule.startLifetimeMultiplier = Mathf.Ceil(2f * halfSize.y / _mainModule.startSpeedMultiplier);
        _emissionModule.rateOverTimeMultiplier = halfSize.x * _emissionPerRadius;

        Play();
    }
}
