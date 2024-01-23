using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextFormatLocalization : TextLocalization
{
    private float _value = 0;

    public void Setup(float value)
    {
        _value = value;
        Setup(Text.text);
    }

    protected override void SetText() => Text.text = string.Format(Localization.Instance.GetText(_keyString), _value);
}
