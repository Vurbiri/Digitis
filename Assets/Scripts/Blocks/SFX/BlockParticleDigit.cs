using Cysharp.Threading.Tasks;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BlockParticleDigit : ParticleSystemController
{
    #region SerializeField
    [Header("Remove")]
    [SerializeField] private int _timeRemoveStart = 1000;
    [SerializeField] private int _timeRemoveNext = 300;
    [Space]
    [SerializeField] private float _radialSpeedRemove = 2f;
    [SerializeField] private float _speedRemove = 4f;
    [SerializeField] private float _sizeRemove = 1.4f;
    [Space]
    [SerializeField] private float _emissionRemoveStart = 15f;
    [SerializeField] private float _emissionRemoveMiddle = 10f;
    [SerializeField] private float _emissionRemoveEnd = 5f;
    [Header("Explode")]
    [SerializeField] private int _timeExplodeStart = 600;
    [SerializeField] private int _timeExplodeNext = 200;
    [Space]
    [SerializeField] private float _speedExplode = 4f;
    [Space]
    [SerializeField] private float _radialSpeedExplodeStart = 2f;
    [SerializeField] private float _radialSpeedExplodeMiddle = -3f;
    [Space]
    [SerializeField] private float _emissionExplodeStart = 14f;
    [SerializeField] private float _emissionExplodeMiddle = 8f;
    [SerializeField] private float _emissionExplodeEnd = 4f;
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

    public void SetupBlock(Material material, Color color) => Setup(material, color, ParticleSystemShapeType.Rectangle);
    public void SetupBomb(Material material, Color color) => Setup(material, color, ParticleSystemShapeType.Circle);
        
    private void Setup(Material material, Color color, ParticleSystemShapeType shapeType)
    {
        ShapeType = shapeType;
        EmissionTimeMultiplier = 1f;
        _mainModule.gravityModifier = 0;
        RadialSpeedMultiplier = 1f;
        SpeedMultiplier = 1.15f;
        SizeMultiplier = 1f;
        Color = color;
        _particleRenderer.sharedMaterial = material;
        Clear();
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

    
}
