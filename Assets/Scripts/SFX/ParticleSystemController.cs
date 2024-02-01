using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemController : MonoBehaviour
{
    protected ParticleSystem _thisParticle;
    protected MainModule _mainModule;
    protected EmissionModule _emissionModule;
    protected VelocityOverLifetimeModule _velocityOverLifetimeModule;
    protected ShapeModule _shapeModule;

    private float _rateOverTimeMultiplier;
    
    private float _radialMultiplier;
    private float _speedModifierMultiplier;

    public Color Color { set => _mainModule.startColor = value; }
    public float Gravity { set => _mainModule.gravityModifier = value; }
    public ParticleSystemShapeType ShapeType { set => _shapeModule.shapeType = value; }
    public float EmissionTimeMultiplier { set => _emissionModule.rateOverTimeMultiplier = _rateOverTimeMultiplier * value; }
    
    public float RadialSpeedMultiplier { set => _velocityOverLifetimeModule.radialMultiplier = _radialMultiplier * value; }
    public float SpeedMultiplier { set => _velocityOverLifetimeModule.speedModifierMultiplier = _speedModifierMultiplier * value; }
    


    protected virtual void Awake()
    {
        _thisParticle = GetComponent<ParticleSystem>();
        
        _mainModule = _thisParticle.main;
        _shapeModule = _thisParticle.shape;
        _emissionModule = _thisParticle.emission;

        _rateOverTimeMultiplier = _emissionModule.rateOverTimeMultiplier;
        

        _velocityOverLifetimeModule = _thisParticle.velocityOverLifetime;
        _radialMultiplier = _velocityOverLifetimeModule.radialMultiplier;
        _speedModifierMultiplier = _velocityOverLifetimeModule.speedModifierMultiplier;
    }

    public void Play() => _thisParticle.Play();
    public void Stop() => _thisParticle.Stop();
    public void Clear() => _thisParticle.Clear();

}
