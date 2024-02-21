using System.Collections;
using UnityEngine;

public abstract class AParticleBackground : AParticleSystemController
{
    [SerializeField] protected CameraReSize _cameraSize;
    [Space]
    [SerializeField] protected float _timeRecolor = 7f;
    [SerializeField] protected Vector3 _minColor = Vector3.one * 0.1f;
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
    }

    protected abstract void OnReSizeParticleSystem(Vector2 halfSize);

    protected IEnumerator ReColorParticlesCoroutine()
    {
        while (true)
        {
            _color.r = Random.Range(_minColor.x, 1f);
            _color.g = Random.Range(_minColor.y, 1f);
            _color.b = Random.Range(_minColor.z, 1f);

            _mainModule.startColor = _color;

            yield return _delay;
        }
    }
}
