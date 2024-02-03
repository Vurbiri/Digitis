using UnityEngine;

public class BlockParticleTrail : ParticleSystemController
{
    [SerializeField] private float _angleShapeEmitterForBomb = 20f;

    public void SetupBlock(Color color) => Setup(color, ParticleSystemShapeType.Rectangle, 360f, 0f);
    public void SetupBomb(Color color) => Setup(color, ParticleSystemShapeType.Circle, _angleShapeEmitterForBomb, 90f - (_angleShapeEmitterForBomb / 2f));

    private void Setup(Color color, ParticleSystemShapeType shapeType, float angle, float rotationZ)
    {
        ShapeType = shapeType;
        _shapeModule.angle = angle;
        _shapeModule.rotation = new(0f, 0f, rotationZ);
        Color = color;
        Clear();
        Stop();
    }

    
}
