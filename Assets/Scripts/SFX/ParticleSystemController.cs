using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemController : MonoBehaviour
{
    protected ParticleSystem _thisParticle;
    protected MainModule _mainModule;
    protected EmissionModule _emissionModule;
    protected ShapeModule _shapeModule;

    protected Color Color { set => _mainModule.startColor = value; }
    protected ParticleSystemShapeType ShapeType { set => _shapeModule.shapeType = value; }

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

}
