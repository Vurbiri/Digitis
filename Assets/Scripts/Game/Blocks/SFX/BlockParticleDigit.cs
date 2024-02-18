using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using System.Threading;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BlockParticleDigit : ABlockParticleSystemController
{
    #region SerializeField
    [SerializeField, Foldout("Block")] private ParticleSystemShapeType _shapeTypeForBlock = ParticleSystemShapeType.Rectangle;
    [SerializeField, Foldout("Block")] private float _radiusThicknessForBlock = 1f;
    [SerializeField, Foldout("Block")] private float _emissionTimeMultiplierForBlock = 1f;
    [SerializeField, Foldout("Block")] private float _radialSpeedMultiplierForBlock = 1f;
    [SerializeField, Foldout("Block")] private float _speedMultiplierForBlock = 1.2f;
    [SerializeField, Foldout("Block")] private float _sizeMultiplierForBlock = 1f;

    [SerializeField, Foldout("Bomb")] private ParticleSystemShapeType _shapeTypeForBomb = ParticleSystemShapeType.Circle;
    [SerializeField, Foldout("Bomb")] private float _arcShapeEmitterForBomb = 12f;
    [SerializeField, Foldout("Bomb")] private float _radiusThicknessForBomb = 0f;
    [SerializeField, Foldout("Bomb")] private float _emissionTimeMultiplierForBomb = 12f;
    [SerializeField, Foldout("Bomb")] private float _radialSpeedMultiplierForBomb = 0f;
    [SerializeField, Foldout("Bomb")] private float _speedMultiplierForBomb = 1.3f;
    [SerializeField, Foldout("Bomb")] private float _sizeMultiplierForBomb = 0.75f;

    [SerializeField, Foldout("Remove")] private int _timeRemoveStart = 1000;
    [SerializeField, Foldout("Remove")] private int _timeRemoveNext = 300;
    [Space]
    [SerializeField, Foldout("Remove")] private float _radialSpeedRemove = 2f;
    [SerializeField, Foldout("Remove")] private float _speedRemove = 4f;
    [SerializeField, Foldout("Remove")] private float _sizeRemove = 1.4f;
    [Space]
    [SerializeField, Foldout("Remove")] private float _emissionRemoveStart = 15f;
    [SerializeField, Foldout("Remove")] private float _emissionRemoveMiddle = 10f;
    [SerializeField, Foldout("Remove")] private float _emissionRemoveEnd = 5f;

    [SerializeField, Foldout("Explode")] private int _timeExplodeStart = 600;
    [SerializeField, Foldout("Explode")] private int _timeExplodeNext = 200;
    [Space]
    [SerializeField, Foldout("Explode")] private float _speedExplode = 4f;
    [Space]
    [SerializeField, Foldout("Explode")] private float _radialSpeedExplodeStart = 2f;
    [SerializeField, Foldout("Explode")] private float _radialSpeedExplodeMiddle = -3f;
    [Space]
    [SerializeField, Foldout("Explode")] private float _emissionExplodeStart = 14f;
    [SerializeField, Foldout("Explode")] private float _emissionExplodeMiddle = 8f;
    [SerializeField, Foldout("Explode")] private float _emissionExplodeEnd = 4f;
    #endregion

    private ParticleSystemRenderer _particleRenderer;
    private VelocityOverLifetimeModule _velocityOverLifetimeModule;
        
    private float _radialMultiplier;
    private float _speedModifierMultiplier;
    private float _sizeMultiplier;
        
    private float RadialSpeedMultiplier { set => _velocityOverLifetimeModule.radialMultiplier = _radialMultiplier * value; }
    private float SpeedMultiplier { set => _velocityOverLifetimeModule.speedModifierMultiplier = _speedModifierMultiplier * value; }
    private float SizeMultiplier { set => _mainModule.startSizeMultiplier = _sizeMultiplier * value; }

    protected override void Awake()
    {
        base.Awake();

        _velocityOverLifetimeModule = _thisParticle.velocityOverLifetime;
        _particleRenderer = GetComponent<ParticleSystemRenderer>();
                
        _radialMultiplier = _velocityOverLifetimeModule.radialMultiplier;
        _speedModifierMultiplier = _velocityOverLifetimeModule.speedModifierMultiplier;
        _sizeMultiplier = _mainModule.startSizeMultiplier;
    }

    public void SetupBlock(Material material, Color color)
    {
        ShapeType = _shapeTypeForBlock;
        _shapeModule.rotation = Vector3.zero;
        _shapeModule.radiusThickness = _radiusThicknessForBlock;
        EmissionTimeMultiplier = _emissionTimeMultiplierForBlock;
        _mainModule.gravityModifier = 0;
        RadialSpeedMultiplier = _radialSpeedMultiplierForBlock;
        SpeedMultiplier = _speedMultiplierForBlock;
        SizeMultiplier = _sizeMultiplierForBlock;
        Color = color;
        _particleRenderer.sharedMaterial = material;
        Clear();
        Play();
    }
    public void SetupBomb(Material material, Color color) 
    {
        ShapeType = _shapeTypeForBomb;
        _shapeModule.arc = _arcShapeEmitterForBomb;
        _shapeModule.rotation = new(0f, 0f, 90f - (_arcShapeEmitterForBomb / 2f));
        _shapeModule.radiusThickness = _radiusThicknessForBomb;
        EmissionTimeMultiplier = _emissionTimeMultiplierForBomb;
        _mainModule.gravityModifier = 0;
        RadialSpeedMultiplier = _radialSpeedMultiplierForBomb;
        SpeedMultiplier = _speedMultiplierForBomb;
        SizeMultiplier = _sizeMultiplierForBomb;
        Color = color;
        _particleRenderer.sharedMaterial = material;
        Clear();
        Play();
    }

    public async UniTask Remove()
    {
        EmissionTimeMultiplier = _emissionRemoveStart;
        RadialSpeedMultiplier = _radialSpeedRemove;
        SpeedMultiplier = _speedRemove;
        SizeMultiplier = _sizeRemove;

        await UniTask.Delay(_timeRemoveStart);

        EmissionTimeMultiplier = _emissionRemoveMiddle;
        _mainModule.gravityModifier = 1f;

        await UniTask.Delay(_timeRemoveNext);

        EmissionTimeMultiplier = _emissionRemoveEnd;

        await UniTask.Delay(_timeRemoveNext);
    }

    public async UniTask Remove(CancellationToken cancellationToken)
    {
        EmissionTimeMultiplier = _emissionRemoveStart;
        RadialSpeedMultiplier = _radialSpeedRemove;
        SpeedMultiplier = _speedRemove;
        SizeMultiplier = _sizeRemove;

        await UniTask.Delay(_timeRemoveStart, cancellationToken: cancellationToken);
        if (cancellationToken.IsCancellationRequested)
            return;

        EmissionTimeMultiplier = _emissionRemoveMiddle;
        _mainModule.gravityModifier = 1f;

        await UniTask.Delay(_timeRemoveNext, cancellationToken: cancellationToken);
        if (cancellationToken.IsCancellationRequested)
            return;

        EmissionTimeMultiplier = _emissionRemoveEnd;

        await UniTask.Delay(_timeRemoveNext, cancellationToken: cancellationToken);
    }

    public async UniTask Explode()
    {
        EmissionTimeMultiplier = _emissionExplodeStart;
        RadialSpeedMultiplier = _radialSpeedExplodeStart;
        SpeedMultiplier = _speedExplode;

        await UniTask.Delay(_timeExplodeStart);

        EmissionTimeMultiplier = _emissionExplodeMiddle;
        RadialSpeedMultiplier = _radialSpeedExplodeMiddle;

        await UniTask.Delay(_timeExplodeNext);

        EmissionTimeMultiplier = _emissionExplodeEnd;

        await UniTask.Delay(_timeExplodeNext);
    }

    public async UniTask ExplodeBomb()
    {
        _shapeModule.rotation = Vector3.zero;
        _shapeModule.radiusThickness = _radiusThicknessForBlock;
        EmissionTimeMultiplier = _emissionRemoveStart;
        RadialSpeedMultiplier = _radialSpeedRemove;
        SpeedMultiplier = _speedRemove;
        SizeMultiplier = _sizeRemove;

        await UniTask.Delay(_timeExplodeNext);

        Stop();
    }

}
