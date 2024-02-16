using UnityEngine;

public abstract class ABlockParticleSystemController : AParticleSystemController
{
    private float _rateOverTimeMultiplier;
    
    protected Color Color { set => _mainModule.startColor = value; }
    protected ParticleSystemShapeType ShapeType { set => _shapeModule.shapeType = value; }
    public float EmissionTimeMultiplier { set => _emissionModule.rateOverTimeMultiplier = _rateOverTimeMultiplier * value; }

    protected override void Awake()
    {
        base.Awake();

        _rateOverTimeMultiplier = _emissionModule.rateOverTimeMultiplier;
    }
}
