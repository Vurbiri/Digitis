using UnityEngine;

public class ParticleBackground : AParticleSystemController
{
    [SerializeField] private Game _game;
    [SerializeField] private CameraSize _cameraSize;
    [Space] 
    [SerializeField] private float _lifePerHeight = 3.4f;
    [SerializeField] private float _emissionPerRadius = 0.25f;
    [Space]
    [SerializeField] private Gradient[] _gradients;
    [SerializeField] private ParticleSystemShapeMultiModeValue[] _modes;

    private int _indexGradient = 0;
    private int _indexMode = 0;

    private void Start()
    {
        RectTransform thisRectTransform = GetComponent<RectTransform>();
        Vector3 positionShape = _shapeModule.position;

        _cameraSize.EventChangingSize += ReSizeParticleSystem;
        _game.EventChangeLevel += _ => ReColorParticleSystem();

        #region Local Functions
        void ReSizeParticleSystem(Vector2 halfSize)
        {
            Clear();
            Stop();

            positionShape.y = halfSize.y;
            _shapeModule.position = positionShape;
            _shapeModule.radius = halfSize.x;

            _mainModule.startLifetimeMultiplier = halfSize.y * _lifePerHeight;
            _mainModule.duration = _mainModule.startLifetimeMultiplier * 5f;

            _emissionModule.rateOverTimeMultiplier = halfSize.x * _emissionPerRadius;

            Play();
        };

        void ReColorParticleSystem()
        {
            _indexGradient = _gradients.RandomIndex(_indexGradient);
            _mainModule.startColor = _gradients[_indexGradient];

            _indexMode = _modes.RandomIndex(_indexMode);
            _shapeModule.radiusMode = _modes[_indexMode];
        }
        #endregion
    }
}
