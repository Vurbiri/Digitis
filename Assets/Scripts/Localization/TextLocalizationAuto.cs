using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextLocalizationAuto : TextLocalization
{
    private void Start() => Setup(Text.text);
}
