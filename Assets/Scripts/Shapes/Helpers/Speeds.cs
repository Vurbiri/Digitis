using UnityEngine;

[System.Serializable]
public class Speeds 
{
    [SerializeField] private float _start = 1f;
    [SerializeField] private float _perLevel = 0.5f;
    [SerializeField] private float _down = 10f;
    [SerializeField] private float _fall = 15f;

    public float Current => _start + _perLevel * level;
    public float Down => _down;
    public float Fall => _fall;

    private const int level = 5;
}
