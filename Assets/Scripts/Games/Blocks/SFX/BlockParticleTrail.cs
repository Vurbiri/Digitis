using NaughtyAttributes;
using UnityEngine;

public class BlockParticleTrail : ABlockParticleSystemController
{
    [SerializeField, Foldout("Block")] private ParticleSystemShapeType _shapeTypeForBlock = ParticleSystemShapeType.Rectangle;

    [SerializeField, Foldout("Bomb")] private ParticleSystemShapeType _shapeTypeForBomb = ParticleSystemShapeType.Circle;
    [SerializeField, Foldout("Bomb")] private float _arcShapeEmitterForBomb = 12f;

    public void SetupBlock(Color color) => Setup(color, _shapeTypeForBlock, 360f, 0f);
    public void SetupBomb(Color color) => Setup(color, _shapeTypeForBomb, _arcShapeEmitterForBomb, 90f - (_arcShapeEmitterForBomb / 2f));

    private void Setup(Color color, ParticleSystemShapeType shapeType, float arc, float rotationZ)
    {
        ShapeType = shapeType;
        _shapeModule.arc = arc;
        _shapeModule.rotation = new(0f, 0f, rotationZ);
        Color = color;
        ClearAndStop();
    }

    
}
