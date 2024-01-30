using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemController : MonoBehaviour
{
    private ParticleSystem _thisParticleSystem;
    private ParticleSystem.MainModule _mainModule;
    private ParticleSystem.EmissionModule _emissionModule;
    private ParticleSystem.VelocityOverLifetimeModule _velocityOverLifetimeModule;
    private ParticleSystem.ShapeModule _shapeModule;

    private float _rateOverTimeMultiplier;
    private float _rateOverDistanceMultiplier;
    private float _radialMultiplier;
    private float _speedModifierMultiplier;

    public Color Color { set => _mainModule.startColor = value; }
    public float Gravity { set => _mainModule.gravityModifier = value; }
    public ParticleSystemShapeType ShapeType { set => _shapeModule.shapeType = value; }
    public float TimeMultiplier { set => _emissionModule.rateOverTimeMultiplier = _rateOverTimeMultiplier * value; }
    public float DistanceMultiplier { set => _emissionModule.rateOverDistanceMultiplier = _rateOverDistanceMultiplier * value; }
    public float RadialSpeedMultiplier { set => _velocityOverLifetimeModule.radialMultiplier = _radialMultiplier * value; }
    public float SpeedMultiplier { set => _velocityOverLifetimeModule.speedModifierMultiplier = _speedModifierMultiplier * value; }
    


    private void Awake()
    {
        _thisParticleSystem = GetComponent<ParticleSystem>();
        
        _mainModule = _thisParticleSystem.main;

        _shapeModule = _thisParticleSystem.shape;

        _emissionModule = _thisParticleSystem.emission;
        _rateOverTimeMultiplier = _emissionModule.rateOverTimeMultiplier;
        _rateOverDistanceMultiplier = _emissionModule.rateOverDistanceMultiplier;

        _velocityOverLifetimeModule = _thisParticleSystem.velocityOverLifetime;
        _radialMultiplier = _velocityOverLifetimeModule.radialMultiplier;
        _speedModifierMultiplier = _velocityOverLifetimeModule.speedModifierMultiplier;
    }

    public void Play() => _thisParticleSystem.Play();
    public void Stop() => _thisParticleSystem.Stop();
    public void Clear() => _thisParticleSystem.Clear();

    
}
