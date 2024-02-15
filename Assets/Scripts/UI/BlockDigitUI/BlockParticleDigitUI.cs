using System.Collections;
using UnityEngine;

public class BlockParticleDigitUI : ABlockParticleSystemController
{
    [SerializeField] private float _timeRunStart = 1f;
    [SerializeField] private float _timeRunNext = 0.35f;
    [Space]
    [SerializeField] private float _emissionRunStart = 1f;
    [SerializeField] private float _emissionRunMiddle = 0.66f;
    [SerializeField] private float _emissionRunEnd = 0.33f;

    private ParticleSystemRenderer _particleRenderer;

    private WaitForSecondsRealtime _pauseRunStart;
    private WaitForSecondsRealtime _pauseRunNext;

    public void SetupBlock(Material material, Color color)
    {
       if(_thisParticle == null)
            base.Awake();

        _pauseRunStart = new(_timeRunStart);
        _pauseRunNext = new(_timeRunNext);

        _particleRenderer = GetComponent<ParticleSystemRenderer>();

        _mainModule.gravityModifier = 0;
        Color = color;
        _particleRenderer.sharedMaterial = material;
    }

    public IEnumerator Run()
    {
        Clear();
        EmissionTimeMultiplier = _emissionRunStart;
        Play();

        yield return _pauseRunStart;

        EmissionTimeMultiplier = _emissionRunMiddle;
        _mainModule.gravityModifier = 1f;

        yield return _pauseRunNext;

        EmissionTimeMultiplier = _emissionRunEnd;

        yield return _pauseRunNext;

        ClearAndStop();
    }
}
