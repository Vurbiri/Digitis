using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextLocalization : MonoBehaviour
{
    public TMP_Text Text {get; protected set;}
    protected string _keyString;    

    private void Awake() => Text = GetComponent<TMP_Text>();

    public void Setup(string keyString)
    {
        _keyString = keyString;
        SetText();
        Localization.Instance.EventSwitchLanguage += SetText;
    }
    private void OnDestroy()
    {
        if(Localization.Instance != null)
            Localization.Instance.EventSwitchLanguage -= SetText;
    }

    protected virtual void SetText() => Text.text = Localization.Instance.GetText(_keyString);
}
