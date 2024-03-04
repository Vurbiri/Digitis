using UnityEngine;

[System.Serializable]
public class Speeds 
{
    [SerializeField] private float _start = 0.5f;
    [SerializeField] private float _perLevel = 0.2f;
    [Space]
    [SerializeField] private float _pseudoSpeedInfinity = 10.5f;
    [Space]
    [SerializeField] private float _down = 17f;
    [SerializeField] private float _fall = 20f;

    public float Current { get; private set; } = 1f;
    public float Down => _down;
    public float Fall => _fall;


    public void Initialize(bool isInfinitySpeed, int level)
    {
        if (isInfinitySpeed)
            Current = _pseudoSpeedInfinity;
        else
            SetSpeed(level);
    }

    public void SetSpeed(int level)
    {
        Current = _start + _perLevel * level;

        if (Current > _down)
            _down = Current + 3f;
        else
            return;

        if (Current > _fall)
            _fall = Current + 3f;
    }
}
