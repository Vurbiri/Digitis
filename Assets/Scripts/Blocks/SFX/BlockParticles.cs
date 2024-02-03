using Cysharp.Threading.Tasks;
using UnityEngine;

public class BlockParticles : MonoBehaviour
{
    [SerializeField] private BlockParticleDigit _particleDigit;
    [SerializeField] private BlockParticleTrail _particleTrail;
    [SerializeField] private ParticleSystem _particleExplode;
   
    public float TrailEmissionTimeMultiplier { set => _particleTrail.EmissionTimeMultiplier = value; }

    public void SetupDigitisBlock(BlockSettings settings)
    {
        _particleDigit.SetupBlock(settings.MaterialParticle, settings.ColorBlock);
        _particleTrail.SetupBlock(settings.ColorBlock);
    }
    public void SetupDigitisBomb(BlockSettings settings)
    {
        _particleDigit.SetupBomb(settings.MaterialParticle, settings.ColorNumber);
        _particleTrail.SetupBomb(settings.ColorNumber);
    }
    public void SetupTetris(Color color, Material particleMaterial)
    {
        _particleDigit.SetupBlock(particleMaterial, color);
        _particleTrail.SetupBlock(color);
    }

    public async UniTask ExplodeBomb()
    {
        _particleExplode.Play();
        await UniTask.WaitUntil(() => _particleExplode.isStopped);
    }

    public UniTask Explode()
    {
        TrailStop();
        return _particleDigit.Explode();
    }

    public UniTask Remove()
    {
        TrailStop();
        return _particleDigit.Remove();
    }

    public void DigitPlay() => _particleDigit.Play();
    public void DigitStop() => _particleDigit.Stop();
    public void DigitClear() => _particleDigit.Clear();

    public void TrailPlay() => _particleTrail.Play();
    public void TrailStop() => _particleTrail.Stop();
}
