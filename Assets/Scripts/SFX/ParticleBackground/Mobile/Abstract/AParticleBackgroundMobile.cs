using UnityEngine;

public abstract class AParticleBackgroundMobile : AParticleBackground
{
    [SerializeField] private float _lifePerHeight = 3.4f;

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

        _mainModule.startLifetimeMultiplier = halfSize.y * _lifePerHeight;
        _mainModule.duration = _mainModule.startLifetimeMultiplier * 5f;

        _emissionModule.rateOverTimeMultiplier = halfSize.x * _emissionPerRadius;

        Play();
    }
}
