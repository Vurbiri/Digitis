using TMPro;
using UnityEngine;

public abstract class ABoard : MonoBehaviour
{
    [SerializeField] protected TMP_Text _textScore;

    public string Value => _textScore.text;

    protected DataGame _dataGame;

    protected virtual void Awake() => _dataGame = DataGame.InstanceF;

    protected void SetText(string value) => _textScore.text = value;
    protected void SetValue(int value) => SetText(value.ToString());
}
