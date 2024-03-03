using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleFullInteractable : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] private Color _textColor = Color.white;
    [SerializeField, Range(0f, 1f)] float _checkmarkAlfaOff = 0.25f;

    private Graphic _checkmark;
    protected Toggle _thisToggle;

    private ColorBlock _colorBlock;

    public bool Interactable
    {
        get => _thisToggle.interactable;
        set
        {
            _thisToggle.interactable = value;
            SetColor();
        }
    }
    public virtual bool IsOn { get => _thisToggle.isOn; set => _thisToggle.isOn = value; }
    public UnityEvent<bool> OnValueChanged => _thisToggle.onValueChanged;

    protected virtual void Awake()
    {
        _thisToggle = GetComponent<Toggle>();
        _colorBlock = _thisToggle.colors;

        _checkmark = _thisToggle.graphic;
        _thisToggle.graphic = null;

        _thisToggle.onValueChanged.AddListener(SetAlfaCheckmark);
        SetColor();
    }

    private void SetColor()
    {
        Color color = _thisToggle.interactable ? _colorBlock.normalColor : _colorBlock.disabledColor;
        _label.color = _textColor * color;

        SetAlfaCheckmark(_thisToggle.isOn);
    }

    private void SetAlfaCheckmark(bool isOn)
    {
        _checkmark.CrossFadeAlpha(Interactable && isOn ? 1f : (isOn ? _checkmarkAlfaOff : 0f) , _colorBlock.fadeDuration, true);
    }
}
