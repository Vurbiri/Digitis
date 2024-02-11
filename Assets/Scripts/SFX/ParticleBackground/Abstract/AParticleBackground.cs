using UnityEngine;

public abstract class AParticleBackground : AParticleSystemController
{
    [SerializeField] private CameraReSize _cameraSize;
    [Space]
    [SerializeField] private float _lifePerHeight = 3.4f;
    [SerializeField] private float _emissionPerRadius = 0.22f;
    [Space]
    [SerializeField] private Gradient[] _gradients;
    [SerializeField] private ParticleSystemShapeMultiModeValue[] _modes;
    [Space]
    [SerializeField] private int _indexGradientStart = 0;
    [SerializeField] private int _indexModeStart = 0;

    private Vector3 _positionShape;

    protected override void Awake()
    {
        base.Awake();
        _positionShape = _shapeModule.position;

        _cameraSize.EventReSize += OnReSizeParticleSystem;
    }

    protected void OnReSizeParticleSystem(Vector2 halfSize)
    {
        Clear();
        Stop();

        _positionShape.y = halfSize.y;
        _shapeModule.position = _positionShape;
        _shapeModule.radius = halfSize.x;

        _mainModule.startLifetimeMultiplier = halfSize.y * _lifePerHeight;
        _mainModule.duration = _mainModule.startLifetimeMultiplier * 5f;

        _emissionModule.rateOverTimeMultiplier = halfSize.x * _emissionPerRadius;

        Play();
    }

    protected void ReColorParticleSystem()
    {
        _indexGradientStart = _gradients.RandomIndex(_indexGradientStart);
        _mainModule.startColor = _gradients[_indexGradientStart];

        _indexModeStart = _modes.RandomIndex(_indexModeStart);
        _shapeModule.radiusMode = _modes[_indexModeStart];
    }
}
