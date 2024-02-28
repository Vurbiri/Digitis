using UnityEngine;
using static UnityEngine.ParticleSystem;


public class MinMax
{
    private readonly float _min;
    private readonly float _max;
    private MinMaxCurve _current;
    private float _multiplier;

    public MinMaxCurve Value => _current;
    public float Min => _current.constantMin;
    public float Max => _current.constantMax;
    public float Multiplier
    {
        get => _multiplier;
        set
        {
            _multiplier = value;
            _current.constantMin = _min * _multiplier;
            _current.constantMax = _max * _multiplier;
        }
    }

    public MinMax() : this(0f, 0f) { }
    public MinMax(float min, float max, float multiplier = 1f)
    {
        _min = min;
        _max = max;
        _multiplier = multiplier;
        _current = new(min * multiplier, max * multiplier)
        {
            mode = ParticleSystemCurveMode.TwoConstants
        };
        
    }

    public MinMaxCurve GetValueMultiplier(float multiplier)
    {
        Multiplier = multiplier;
        return _current;
    }

    public static implicit operator MinMax(MinMaxCurve value) => new(value.constantMin, value.constantMax);
    public static implicit operator MinMaxCurve(MinMax obj) => obj.Value;
}
