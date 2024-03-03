using UnityEngine;

[System.Serializable]
public class Speeds 
{
    [SerializeField] private float _start = 0.5f;
    [SerializeField] private float _perLevel = 0.2f;
    [Space]
    [SerializeField] private float _forInfinity = 0.4f;
    [Space]
    [SerializeField] private float _down = 17f;
    [SerializeField] private float _fall = 20f;

    private bool _isInfinitySpeed = false;

    public float Current { get; private set; } = 1f;
    public float Down => _down;
    public float Fall => _fall;

    public bool IsInfinitySpeed
    {
        set
        {
            _isInfinitySpeed = value;

            if (value)
                Current = _forInfinity;
        }
    }

    public int Level 
    { 
        set 
        {
            if (_isInfinitySpeed)
                return;

            Current = _start + _perLevel * value;

            if (Current > _down)
                _down = Current + 3f;
            else
                return;

            if (Current > _fall)
                _fall = Current + 3f;
        } 
    }
}
