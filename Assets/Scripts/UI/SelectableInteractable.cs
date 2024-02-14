using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class SelectableInteractable : MonoBehaviour
{
    [SerializeField] private Graphic[] _children;
    [SerializeField] private TMP_Text[] _texts;
    [SerializeField] Color _textColor = Color.white;

    public bool Interactable 
    { 
        get => ThisSelectable.interactable;
        set
        {
            ThisSelectable.interactable = value;
            SetIColor();
        }
    }
    public Selectable ThisSelectable { get; private set; }

    private ColorBlock _colorBlock;

    private void Awake()
    {
        ThisSelectable = GetComponent<Selectable>();
        _colorBlock = ThisSelectable.colors;

        SetIColor();
    }

    private void SetIColor()
    {
        Color color = ThisSelectable.interactable ? _colorBlock.normalColor : _colorBlock.disabledColor;

        foreach (var child in _children)
            child.CrossFadeColor(color, _colorBlock.fadeDuration, true, true);
        foreach (var text in _texts)
            text.color = _textColor * color;
    }
}
