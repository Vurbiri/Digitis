using static UnityEngine.ParticleSystem;

public class MinMaxVector3
{
    private readonly MinMax _x;
    private readonly MinMax _y;
    private readonly MinMax _z;

    public MinMaxCurve X => _x;
    public MinMaxCurve Y => _y;
    public MinMaxCurve Z => _z;

    public float Multiplier
    {
        set
        {
            _x.Multiplier = value;
            _y.Multiplier = value;
            _z.Multiplier = value;
        }
    }

    public MinMaxVector3(MinMaxCurve x, MinMaxCurve y, MinMaxCurve z, float multiplier = 1f)
    {
        _x = x;
        _y = y;
        _z = z;
        Multiplier = multiplier;
    }
}
