using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BlockVisual))]
public class BlockSFX : MonoBehaviour
{
    [SerializeField] private BlockParticles _particles;

    private BlockVisual _blockVisual;

    private void Awake()
    {
        _blockVisual = GetComponent<BlockVisual>();
    }

    public void SetupDigitisBlock(BlockSettings settings)
    {
        _blockVisual.SetupDigitisBlock(settings);
        _particles.SetupDigitisBlock(settings);
    }
    public void SetupDigitisBomb(BlockSettings settings)
    {
        _blockVisual.SetupDigitisBomb(settings);
        _particles.SetupDigitisBomb(settings);
    }
    public void SetupTetris(Color color, Sprite sprite, Material particleMaterial)
    {
        _blockVisual.SetupTetris(color, sprite);
        _particles.SetupTetris(color, particleMaterial);
    }

    public void Transfer()
    {
        _particles.DigitClear();
        _particles.DigitStop();
    }
    public void StartFall(float speed)
    {
        SetTrailDistanceMultiplier(speed);
        _particles.DigitStop();
        _particles.TrailPlay();
    }
    public void Fixed()
    {
        SetTrailDistanceMultiplier(0f);
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

    public void SetTrailDistanceMultiplier(float rate) => _particles.TrailEmissionDistanceMultiplier = rate;

}
