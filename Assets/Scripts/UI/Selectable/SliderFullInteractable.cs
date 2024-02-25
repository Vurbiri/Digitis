using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderFullInteractable : MonoBehaviour
{
    [SerializeField] private Graphic[] _children;
    [SerializeField] private TMP_Text[] _texts;
    [SerializeField] Color _textColor = Color.white;

    private Slider _thisSlider;

    public bool Interactable 
    { 
        get => _thisSlider.interactable;
        set
        {
            _thisSlider.interactable = value;
            SetIColor();
        }
    }
    public float Value { get => _thisSlider.value; set => _thisSlider.value = value; }
    public float MinValue { get => _thisSlider.minValue; set => _thisSlider.minValue = value; }

    private ColorBlock _colorBlock;

    private void Awake()
    {
        _thisSlider = GetComponent<Slider>();
        _colorBlock = _thisSlider.colors;

        SetIColor();
    }

    private void SetIColor()
    {
        Color color = Interactable ? _colorBlock.normalColor : _colorBlock.disabledColor;

        foreach (var child in _children)
            child.CrossFadeColor(color, _colorBlock.fadeDuration, true, true);
        foreach (var text in _texts)
            text.color = _textColor * color;
    }
}
