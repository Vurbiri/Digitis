using UnityEngine;

public abstract class AParticleBackground : AParticleSystemController
{
    [SerializeField] protected CameraReSize _cameraSize;
    [Space]
    [SerializeField] protected Gradient[] _gradients;
    [SerializeField] protected int _indexGradientStart = 0;
    [Space]
    [SerializeField] protected float _emissionPerRadius = 0.22f;


    protected override void Awake()
    {
        base.Awake();

        _mainModule.duration = _mainModule.startLifetimeMultiplier * 5f;
        _cameraSize.EventReSize += OnReSizeParticleSystem;

        Play();
    }

    protected abstract void OnReSizeParticleSystem(Vector2 halfSize);

    protected void ReColorParticleSystem()
    {
        _indexGradientStart = _gradients.RandomIndex(_indexGradientStart);
        _mainModule.startColor = _gradients[_indexGradientStart];
    }
}
