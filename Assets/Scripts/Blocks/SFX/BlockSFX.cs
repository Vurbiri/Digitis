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

    public void SetupDigitis(BlockSettings settings)
    {
        _blockVisual.SetupDigitis(settings);

        if (settings.Digit == 0)
            _particles.SetupDigitisBomb(settings);
        else
            _particles.SetupDigitisBlock(settings);
    }

    public void SetupTetris(Color color, Sprite sprite)
    {
        _blockVisual.SetupTetris(color, sprite);
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


    public void DigitPlay() => _particles.DigitPlay();
    public void DigitStop() => _particles.DigitStop();
    public void TrailPlay() => _particles.TrailPlay();
    public void TrailStop() => _particles.TrailStop();
    public void SetTrailDistanceMultiplier(float rate) => _particles.TrailEmissionDistanceMultiplier = rate;

}
