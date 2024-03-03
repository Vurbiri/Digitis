using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(ParticleSystem))]
public abstract class AParticleSystemController : MonoBehaviour
{
    protected ParticleSystem _thisParticle;
    protected MainModule _mainModule;
    protected EmissionModule _emissionModule;
    protected ShapeModule _shapeModule;

    protected virtual void Awake()
    {
        _thisParticle = GetComponent<ParticleSystem>();
        
        _mainModule = _thisParticle.main;
        _emissionModule = _thisParticle.emission;
        _shapeModule = _thisParticle.shape;
    }

    public void Play() => _thisParticle.Play();
    public void Stop() => _thisParticle.Stop();
    public void Clear() => _thisParticle.Clear();

    public void ClearAndStop()
    {
        _thisParticle.Clear();
        _thisParticle.Stop();
    }
}
