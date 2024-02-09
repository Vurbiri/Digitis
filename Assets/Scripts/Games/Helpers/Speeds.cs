using UnityEngine;

[System.Serializable]
public class Speeds 
{
    [SerializeField] private float _start = 1f;
    [SerializeField] private float _perLevel = 0.5f;
    [SerializeField] private float _down = 15f;
    [SerializeField] private float _fall = 20f;

    public float Current { get; private set; } = 1f;
    public float Down => _down;
    public float Fall => _fall;

    public int Level 
    { 
        set 
        {
            Current = _start + _perLevel * value;

            if (Current > _down)
                _down = Current + 2.5f;
            else
                return;

            if (Current > _fall)
                _fall = Current + 2.5f;

        } 
    }
}
