using UnityEngine;

public class Chance
{
    private const float _baseChance = 100f;
    private readonly float _startChance;
    private float _entropy;

    public bool Next => NextChance(_startChance);

    public bool NextIncrement(float addChance) => NextChance(_startChance + addChance);

    private bool NextChance(float chance)
    {
        if (chance <= 0) return false;
        else if (chance >= _baseChance) return true;

        _entropy -= chance;
        float currentChance = Random.Range(0f, _baseChance);
        bool isChance = (currentChance <= chance || _entropy <= 0) && _entropy <= _baseChance;
        if (isChance) _entropy += _baseChance;

        return isChance;
    }

    public Chance(float startChance)
    {
        _startChance = startChance;
        _entropy = Random.Range(_baseChance * 0.25f, _baseChance * 0.75f );
    }


}
