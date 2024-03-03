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
    [Space]
    [SerializeField] private Color _colorGameOver;
    [SerializeField] protected float _speedGameOver = 0.6f;

    private Color _color = Color.white;
    private WaitForSecondsRealtime _delay;
    private Coroutine _coroutine;

    protected override void Awake()
    {
        base.Awake();

        _delay = new(_timeRecolor);
        StartReColorParticlesCoroutine();

        _cameraSize.EventReSize += OnReSizeParticleSystem;

        Play();
    }

    protected abstract void OnReSizeParticleSystem(Vector2 halfSize);

    protected void StartReColorParticlesCoroutine()
    {
        StopReColorParticlesCoroutine();
        _coroutine = StartCoroutine(ReColorParticlesCoroutine());

        IEnumerator ReColorParticlesCoroutine()
        {
            while (true)
            {
                _color.Random();
                _mainModule.startColor = _color;

                yield return _delay;
            }
        }
    }

    protected void StopReColorParticlesCoroutine()
    {
        if (_coroutine == null)
            return;

        StopCoroutine(_coroutine);
        _coroutine = null;
    }

    protected void OnGameOver()
    {
        StopReColorParticlesCoroutine();
        ClearAndStop();
        SetSpeed(_speedGameOver);
        _mainModule.startColor = _colorGameOver;
        _emissionPerRadius *= 2f;
        _emissionModule.rateOverTimeMultiplier *= 2f;
        Play();
    }

    protected void SetSpeed(float speed)
    {
        _mainModule.startSpeedMultiplier = speed;
        _emissionModule.rateOverTimeMultiplier = _cameraSize.Size.x * _emissionPerRadius * speed / 2f;
        _mainModule.startLifetimeMultiplier = Mathf.Ceil(_cameraSize.Size.y / speed);
    }
}
