using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using System.Threading;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BlockParticleDigit : ABlockParticleSystemController
{
    #region SerializeField
    [SerializeField, Foldout("Block")] private ParticleSystemShapeType _shapeTypeForBlock = ParticleSystemShapeType.Rectangle;
    [SerializeField, Foldout("Block")] private float _arcShapeEmitterForBlock = 360f;
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
    [SerializeField, Foldout("Remove")] private float _sizeRemove = 1.4f;
    [SerializeField, Foldout("Remove")] private float _gravityRemove = 1.4f;
    [Space]
    [SerializeField, Foldout("Remove")] private float _speedRemoveStart = 0f;
    [SerializeField, Foldout("Remove")] private float _speedRemoveMiddle = 2f;
    [Space]
    [SerializeField, Foldout("Remove")] private float _emissionRemoveStart = 15f;
    [SerializeField, Foldout("Remove")] private float _emissionRemoveMiddle = 10f;
    [SerializeField, Foldout("Remove")] private float _emissionRemoveEnd = 5f;

    [SerializeField, Foldout("Explode")] private int _timeExplodeStart = 600;
    [SerializeField, Foldout("Explode")] private int _timeExplodeNext = 200;
    [Space]
    [SerializeField, Foldout("Explode")] private float _speedExplodeStart = 0f;
    [SerializeField, Foldout("Explode")] private float _speedExplodeMiddle = 4f;
    [Space]
    [SerializeField, Foldout("Explode")] private float _radialSpeedExplodeStart = 2f;
    [SerializeField, Foldout("Explode")] private float _radialSpeedExplodeMiddle = -3f;
    [Space]
    [SerializeField, Foldout("Explode")] private float _emissionExplodeStart = 14f;
    [SerializeField, Foldout("Explode")] private float _emissionExplodeMiddle = 8f;
    [SerializeField, Foldout("Explode")] private float _emissionExplodeEnd = 4f;

    [SerializeField, Foldout("ExplodeBomb")] private int _timeExplodeBombStart = 500;
    [Space]
    [SerializeField, Foldout("ExplodeBomb")] private float _sizeExplodeBomb = 1f;
    [SerializeField, Foldout("ExplodeBomb")] private float _speedExplodeBomb = 0f;
    [SerializeField, Foldout("ExplodeBomb")] private float _radialSpeedExplodeBomb = 3f;
    [SerializeField, Foldout("ExplodeBomb")] private float _emissionExplodeBomb = 20f;
    #endregion

    private ParticleSystemRenderer _particleRenderer;
    private VelocityOverLifetimeModule _velocityOverLifetimeModule;
        
    private float _radialMultiplier;
    private MinMax _sizeMultiplier;
    private MinMaxVector3 _speedLinear;


    private float SizeMultiplier { set => _mainModule.startSize = _sizeMultiplier.GetValueMultiplier(value); }
    private float RadialSpeedMultiplier { set => _velocityOverLifetimeModule.radialMultiplier = _radialMultiplier * value; }
    private float SpeedLinearMultiplier 
    {
        set
        {
            _speedLinear.Multiplier = value;
            _velocityOverLifetimeModule.SetLinearFromMinMaxVector3(_speedLinear);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _velocityOverLifetimeModule = _thisParticle.velocityOverLifetime;
        _particleRenderer = GetComponent<ParticleSystemRenderer>();
                
        _radialMultiplier = _velocityOverLifetimeModule.radialMultiplier;

        _sizeMultiplier = _mainModule.startSize;

        _speedLinear = _velocityOverLifetimeModule.LinearToMinMaxVector3();
    }

    public void SetupBlock(Material material, Color color)
    {
        ShapeType = _shapeTypeForBlock;
        _shapeModule.arc = _arcShapeEmitterForBlock;
        _shapeModule.rotation = Vector3.zero;
        _shapeModule.radiusThickness = _radiusThicknessForBlock;
        EmissionTimeMultiplier = _emissionTimeMultiplierForBlock;
        _mainModule.gravityModifier = 0;
        RadialSpeedMultiplier = _radialSpeedMultiplierForBlock;
        SpeedLinearMultiplier = _speedMultiplierForBlock;
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
        SpeedLinearMultiplier = _speedMultiplierForBomb;
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
        SpeedLinearMultiplier = _speedRemoveStart;
        SizeMultiplier = _sizeRemove;

        await UniTask.Delay(_timeRemoveStart);

        EmissionTimeMultiplier = _emissionRemoveMiddle;
        SpeedLinearMultiplier = _speedRemoveMiddle;
        _mainModule.gravityModifier = _gravityRemove;

        await UniTask.Delay(_timeRemoveNext);

        EmissionTimeMultiplier = _emissionRemoveEnd;

        await UniTask.Delay(_timeRemoveNext);
    }
    public async UniTask Remove(CancellationToken cancellationToken)
    {
        EmissionTimeMultiplier = _emissionRemoveStart;
        RadialSpeedMultiplier = _radialSpeedRemove;
        SpeedLinearMultiplier = _speedRemoveStart;
        SizeMultiplier = _sizeRemove;

        await UniTask.Delay(_timeRemoveStart, cancellationToken: cancellationToken);
        if (cancellationToken.IsCancellationRequested)
            return;

        EmissionTimeMultiplier = _emissionRemoveMiddle;
        SpeedLinearMultiplier = _speedRemoveMiddle;
        _mainModule.gravityModifier = _gravityRemove;

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
        SpeedLinearMultiplier = _speedExplodeStart;

        await UniTask.Delay(_timeExplodeStart);

        EmissionTimeMultiplier = _emissionExplodeMiddle;
        SpeedLinearMultiplier = _speedExplodeMiddle;
        RadialSpeedMultiplier = _radialSpeedExplodeMiddle;

        await UniTask.Delay(_timeExplodeNext);

        EmissionTimeMultiplier = _emissionExplodeEnd;

        await UniTask.Delay(_timeExplodeNext);
    }

    public async UniTask ExplodeBomb()
    {
        _shapeModule.rotation = Vector3.zero;
        _shapeModule.arc = _arcShapeEmitterForBlock;
        _shapeModule.radiusThickness = _radiusThicknessForBlock;
        EmissionTimeMultiplier = _emissionExplodeBomb;
        RadialSpeedMultiplier = _radialSpeedExplodeBomb;
        SpeedLinearMultiplier = _speedExplodeBomb;
        SizeMultiplier = _sizeExplodeBomb;

        await UniTask.Delay(_timeExplodeBombStart);

        Stop();
    }
/*
#if UNITY_EDITOR
    [Button]
    public void TestRemove()
    {
        Init();

        TestRemoveAsync().Forget();

        async UniTaskVoid TestRemoveAsync()
        {
            EmissionTimeMultiplier = _emissionRemoveStart;
            RadialSpeedMultiplier = _radialSpeedRemove;
            SpeedLinearMultiplier = _speedRemoveStart;
            SizeMultiplier = _sizeRemove;

            await UniTask.Delay(_timeRemoveStart);

            EmissionTimeMultiplier = _emissionRemoveMiddle;
            SpeedLinearMultiplier = _speedRemoveMiddle;
            _mainModule.gravityModifier = _gravityRemove;

            await UniTask.Delay(_timeRemoveNext);

            EmissionTimeMultiplier = _emissionRemoveEnd;

            await UniTask.Delay(_timeRemoveNext);

            Reset();
        }
    }
    [Button]
    public void TestExplode()
    {
        Init();

        TestExplodeAsync().Forget();

        async UniTaskVoid TestExplodeAsync()
        {
            EmissionTimeMultiplier = _emissionExplodeStart;
            RadialSpeedMultiplier = _radialSpeedExplodeStart;
            SpeedLinearMultiplier = _speedExplodeStart;

            await UniTask.Delay(_timeExplodeStart);

            EmissionTimeMultiplier = _emissionExplodeMiddle;
            SpeedLinearMultiplier = _speedExplodeMiddle;
            RadialSpeedMultiplier = _radialSpeedExplodeMiddle;

            await UniTask.Delay(_timeExplodeNext);

            EmissionTimeMultiplier = _emissionExplodeEnd;

            await UniTask.Delay(_timeExplodeNext);

            Reset();
        }
    }
    [Button]
    public void TestExplodeBomb()
    {
        Init();
        ResetBomb();

        ExplodeBomb().Forget();

        async UniTaskVoid ExplodeBomb()
        {
            _shapeModule.rotation = Vector3.zero;
            _shapeModule.arc = _arcShapeEmitterForBlock;
            _shapeModule.radiusThickness = _radiusThicknessForBlock;
            EmissionTimeMultiplier = _emissionExplodeBomb;
            RadialSpeedMultiplier = _radialSpeedExplodeBomb;
            SpeedLinearMultiplier = _speedExplodeBomb;
            SizeMultiplier = _sizeExplodeBomb;

            await UniTask.Delay(_timeExplodeBombStart);

            Reset();
        }
    }
    [Button]
    public void Reset()
    {
        Init();

        ShapeType = _shapeTypeForBlock;
        _shapeModule.arc = _arcShapeEmitterForBlock;
        _shapeModule.rotation = Vector3.zero;
        _shapeModule.radiusThickness = _radiusThicknessForBlock;
        EmissionTimeMultiplier = _emissionTimeMultiplierForBlock;
        _mainModule.gravityModifier = 0;
        RadialSpeedMultiplier = _radialSpeedMultiplierForBlock;
        SpeedLinearMultiplier = _speedMultiplierForBlock;
        SizeMultiplier = _sizeMultiplierForBlock;
        Clear();
        Play();
    }
    [Button]
    public void ResetBomb()
    {
        Init();


        ShapeType = _shapeTypeForBomb;
        _shapeModule.arc = _arcShapeEmitterForBomb;
        _shapeModule.rotation = new(0f, 0f, 90f - (_arcShapeEmitterForBomb / 2f));
        _shapeModule.radiusThickness = _radiusThicknessForBomb;
        EmissionTimeMultiplier = _emissionTimeMultiplierForBomb;
        _mainModule.gravityModifier = 0;
        RadialSpeedMultiplier = _radialSpeedMultiplierForBomb;
        SpeedLinearMultiplier = _speedMultiplierForBomb;
        SizeMultiplier = _sizeMultiplierForBomb;
        Clear();
        Play();
    }

    private void Init()
    {
        base.Awake();

        _velocityOverLifetimeModule = _thisParticle.velocityOverLifetime;
        _particleRenderer = GetComponent<ParticleSystemRenderer>();

        _emissionModule.rateOverTimeMultiplier = 3f;

        _rateOverTimeMultiplier = 3f;
        _radialMultiplier = -0.75f;

        _sizeMultiplier = new(0.2f, 0.45f);

        _speedLinear = new(new(-1.25f, 1.25f), new(-1.25f, 1.25f), new(0f, 0f));
    }

#endif
*/
}
