using Cysharp.Threading.Tasks;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BlockParticleDigit : ParticleSystemController
{

    private ParticleSystemRenderer _particleRenderer;

    protected override void Awake()
    {
        base.Awake();

        _particleRenderer = GetComponent<ParticleSystemRenderer>();
    }

    public void SetupDigitisBlock(Material material, Color color)
    {
        ShapeType = ParticleSystemShapeType.Rectangle;
        EmissionTimeMultiplier = 1f;
        Gravity = 0;
        RadialSpeedMultiplier = 1f;
        SpeedMultiplier = 1f;

        Color = color;
        _particleRenderer.sharedMaterial = material;
        Play();
    }

    public void SetupDigitisBomb()
    {
        Stop();
        Clear();
    }


    public async UniTask Explode()
    {
        EmissionTimeMultiplier = 15f;
        RadialSpeedMultiplier = 2f;
        SpeedMultiplier = 4f;
        //Play();

        await UniTask.Delay(500);

        EmissionTimeMultiplier = 10f;
        RadialSpeedMultiplier = -2f;

        await UniTask.Delay(300);

        EmissionTimeMultiplier = 5f;
        RadialSpeedMultiplier = -4f;

        await UniTask.Delay(300);
        //Stop();
    }

    public async UniTask Remove()
    {
        EmissionTimeMultiplier = 15f;
        RadialSpeedMultiplier = 2f;
        SpeedMultiplier = 4f;
        //Play();

        await UniTask.Delay(1000);

        EmissionTimeMultiplier = 10f;
        Gravity = 1f;

        await UniTask.Delay(300);

        EmissionTimeMultiplier = 5f;

        await UniTask.Delay(300);
        //Stop();
    }
}
