using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class BlockParticles : MonoBehaviour
{
    [SerializeField] private BlockParticleDigit _particleDigit;
    [SerializeField] private BlockParticleTrail _particleTrail;
    [SerializeField] private ParticleSystem _particleExplode;
   
    public float TrailEmissionTimeMultiplier { set => _particleTrail.EmissionTimeMultiplier = value; }

    public void SetupBlock(BlockSettings settings)
    {
        _particleDigit.SetupBlock(settings.MaterialParticle, settings.ColorBlock);
        _particleTrail.SetupBlock(settings.ColorBlock);
    }
    public void SetupBomb(BlockSettings settings)
    {
        _particleDigit.SetupBomb(settings.MaterialParticle, settings.ColorNumber);
        _particleTrail.SetupBomb(settings.ColorNumber);
    }

    public UniTask ExplodeBomb()
    {
        _particleExplode.Play();
        return UniTask.WhenAll(UniTask.WaitUntil(() => _particleExplode.isStopped), _particleDigit.ExplodeBomb());
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

    public UniTask Remove(CancellationToken cancellationToken)
    {
        return _particleDigit.Remove(cancellationToken);
    }

    public void DigitPlay() => _particleDigit.Play();
    public void DigitStop() => _particleDigit.Stop();
    public void DigitClearAndStop() => _particleDigit.ClearAndStop();

    public void TrailPlay() => _particleTrail.Play();
    public void TrailStop() => _particleTrail.Stop();
}
