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

    public void SetupDigitisBlock(BlockSettings settings)
    {
        _blockVisual.SetupDigitis(settings);
        _particles.SetupDigitisBlock(settings);
    }
    public void SetupDigitisBomb(BlockSettings settings)
    {
        _blockVisual.SetupDigitis(settings);
        _particles.SetupDigitisBomb(settings);
    }
    public void SetupTetris(ShapeTetris shapeTetris, Material particleMaterial)
    {
        _blockVisual.SetupTetris(shapeTetris.ColorBlock, shapeTetris.SpriteBlock);
        _particles.SetupTetris(shapeTetris.ColorBlock, particleMaterial);
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
