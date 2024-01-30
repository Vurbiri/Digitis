using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BlockVisual))]
public class BlockSFX : MonoBehaviour
{
    [SerializeField] private ParticleSystemController _psTrail;

    private BlockVisual _blockVisual;

    private void Awake()
    {
        _blockVisual = GetComponent<BlockVisual>();
    }

    public void Setup(BlockSettings settings)
    {
        _blockVisual.Setup(settings);

        if (settings.Digit == 0)
        {
            _psTrail.ShapeType = ParticleSystemShapeType.Circle;
            _psTrail.Color = settings.ColorNumber;

            _psTrail.TimeMultiplier = 0.5f;
        }
        else
        {
            _psTrail.ShapeType = ParticleSystemShapeType.Box;
            _psTrail.Color = settings.ColorBlock;

            _psTrail.TimeMultiplier = 1f;
        }

        _psTrail.DistanceMultiplier = 0f;
        _psTrail.Gravity = 0;
        
        _psTrail.RadialSpeedMultiplier = 1f;
        _psTrail.SpeedMultiplier = 1f;
    }

    public async UniTask BlockOff()
    {
        _psTrail.TimeMultiplier = 15f;
        _psTrail.RadialSpeedMultiplier = 2f;
        _psTrail.SpeedMultiplier = 4f;
        _blockVisual.Off();
        await UniTask.Delay(800);
        _psTrail.TimeMultiplier = 10f;
        _psTrail.Gravity = 1f;
        await UniTask.Delay(300);
        _psTrail.TimeMultiplier = 5f;
        await UniTask.Delay(300);
    }

    public void TrailPlay() => _psTrail.Play();
    public void TrailStop() => _psTrail.Stop();
    public void SetTrailDistanceMultiplier(float rate) => _psTrail.DistanceMultiplier = rate;
   
}
