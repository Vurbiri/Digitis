using Cysharp.Threading.Tasks;
using UnityEngine;

public class BlockParticles : MonoBehaviour
{
    [SerializeField] private BlockParticleDigit _particleDigit;
    [SerializeField] private BlockParticleTrail _particleTrail;
    [SerializeField] private ParticleSystem _particleExplode;

    public float TrailEmissionDistanceMultiplier { set => _particleTrail.EmissionDistanceMultiplier = value; }

    public void SetupDigitisBlock(BlockSettings settings)
    {
        _particleDigit.SetupDigitisBlock(settings.MaterialParticle, settings.ColorBlock);
        _particleTrail.SetupDigitisBlock(settings.ColorBlock);
    }

    public void SetupDigitisBomb(BlockSettings settings)
    {
        _particleDigit.SetupDigitisBomb();
        _particleTrail.SetupDigitisBomb(settings.ColorNumber);
    }

    public async UniTask ExplodeBomb()
    {
        _particleExplode.Play();
        await UniTask.WaitUntil(() => _particleExplode.isStopped);
    }

    public UniTask Explode()
    {
        return _particleDigit.Explode();
    }

    public UniTask Remove()
    {
        return _particleDigit.Remove();
    }

    public void DigitPlay() => _particleDigit.Play();
    public void DigitStop() => _particleDigit.Stop();
    public void TrailPlay() => _particleTrail.Play();
    public void TrailStop() => _particleTrail.Stop();
}
