using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BlockSprites))]
public class BlockSFX : MonoBehaviour
{
    [SerializeField] private BlockParticles _particles;

    private BlockSprites _blockVisual;

    private void Awake()
    {
        _blockVisual = GetComponent<BlockSprites>();
    }

    public void SetupBlock(BlockSettings settings)
    {
        _blockVisual.Setup(settings);
        _particles.SetupBlock(settings);
    }
    public void SetupBomb(BlockSettings settings)
    {
        _blockVisual.Setup(settings);
        _particles.SetupBomb(settings);
    }

    public void Transfer()
    {
        _particles.DigitClear();
        _particles.DigitStop();
    }
    public void StartFall(float speed)
    {
        SetTrailEmissionTimeMultiplier(speed);
        _particles.DigitStop();
        _particles.TrailPlay();
    }
    public void Fixed()
    {
        SetTrailEmissionTimeMultiplier(0f);
        _particles.TrailStop();
        _particles.DigitPlay();
    }

    public async UniTask ExplodeBomb()
    {
        _blockVisual.Off();
        await _particles.ExplodeBomb();
    }

    public async UniTask Explode()
    {
        _blockVisual.Off();
        await _particles.Explode();
    }

    public async UniTask Remove()
    {
        _blockVisual.Off();
        await _particles.Remove();
    }

    public void SetTrailEmissionTimeMultiplier(float rate) => _particles.TrailEmissionTimeMultiplier = rate;

}
