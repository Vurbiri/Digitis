using TMPro;
using UnityEngine;

public abstract class ABoard : MonoBehaviour
{
    [SerializeField] protected Game _game;
    [SerializeField] protected TMP_Text _textScore;

    protected void SetValue(string value) => _textScore.text = value;
}
