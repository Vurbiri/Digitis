using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Toggle;

[RequireComponent(typeof(Toggle))]
public class ToggleFullInteractable : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] Color _textColor = Color.white;

    private Graphic _checkmark;
    private Color _checkmarkColor = Color.white;
    private Toggle _thisToggle;

    public bool Interactable
    {
        get => _thisToggle.interactable;
        set
        {
            _thisToggle.interactable = value;
            SetIColor();
        }
    }
    public bool IsOn { get => _thisToggle.isOn; set => _thisToggle.isOn = value; }
    public ToggleEvent OnValueChanged => _thisToggle.onValueChanged;

    private ColorBlock _colorBlock;

    private void Awake()
    {
        _thisToggle = GetComponent<Toggle>();
        _colorBlock = _thisToggle.colors;

        _checkmark = _thisToggle.graphic;
        _checkmarkColor = _checkmark.color;

        SetIColor();
    }

    private void SetIColor()
    {
        Color color = Interactable ? _colorBlock.normalColor : _colorBlock.disabledColor;
        _label.color = _textColor * color;

        if(Interactable)
            _checkmarkColor.a = IsOn ? 1f : 0f;
        else
            _checkmarkColor.a = 0f;
        _checkmark.CrossFadeColor(_checkmarkColor, _colorBlock.fadeDuration, true, true);


    }
}
