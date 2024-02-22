using System.Collections;
using UnityEngine;

public abstract class AParticleBackground : AParticleSystemController
{
    [SerializeField] protected CameraReSize _cameraSize;
    [Space]
    [SerializeField] protected float _timeRecolor = 7f;
    [SerializeField] protected float _spreadColor = 0.2f;
    [Space]
    [SerializeField] protected float _emissionPerRadius = 0.22f;

    private Color _color = Color.white;
    private WaitForSecondsRealtime _delay;

    protected override void Awake()
    {
        base.Awake();

        _delay = new(_timeRecolor);
        StartCoroutine(ReColorParticlesCoroutine());

        _cameraSize.EventReSize += OnReSizeParticleSystem;

        Play();
    }

    protected abstract void OnReSizeParticleSystem(Vector2 halfSize);

    protected IEnumerator ReColorParticlesCoroutine()
    {
        while (true)
        {
            _color.Random();
            _mainModule.startColor = _color;

            yield return _delay;
        }
    }
}
