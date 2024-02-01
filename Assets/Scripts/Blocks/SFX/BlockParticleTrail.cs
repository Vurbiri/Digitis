using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BlockParticleTrail : ParticleSystemController
{

    private float _rateOverDistanceMultiplier;

    public float EmissionDistanceMultiplier { set => _emissionModule.rateOverDistanceMultiplier = _rateOverDistanceMultiplier * value; }

    protected override void Awake()
    {
        base.Awake();

        _rateOverDistanceMultiplier = _emissionModule.rateOverDistanceMultiplier;
    }

    public void SetupDigitisBlock(Color color)
    {
        ShapeType = ParticleSystemShapeType.Rectangle;
        _shapeModule.scale = Vector3.one;
        Color = color;
    }

    public void SetupDigitisBomb(Color color)
    {
        ShapeType = ParticleSystemShapeType.Circle;
        _shapeModule.scale = Vector3.one * 0.25f;
        Color = color;
    }
}
