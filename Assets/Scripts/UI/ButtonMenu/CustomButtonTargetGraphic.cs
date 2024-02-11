using UnityEngine;
using UnityEngine.UI;

public class CustomButtonTargetGraphic : MonoBehaviour
{
    [SerializeField] private Image[] _targetImages;
    [SerializeField] private Animator[] _targetAnimators;
    [Space]
    [SerializeField] private ColorBlock _colorBlock = ColorBlock.defaultColorBlock;

    public void SetNormalState(bool instant)
    {
        SetColor(_colorBlock.normalColor, instant);
    }

    public void SetHighlightedState(bool instant)
    {
        SetColor(_colorBlock.highlightedColor, instant);
    }

    public void SetPressedState(bool instant)
    {
        SetColor(_colorBlock.pressedColor, instant);
    }

    public void SetSelectedState(bool instant)
    {
        SetColor(_colorBlock.selectedColor, instant);
    }

    public void SetDisabledState(bool instant)
    {
        SetColor(_colorBlock.disabledColor, instant);
    }



    private void SetColor(Color targetColor, bool instant)
    {
       foreach (var image in _targetImages) 
            image.CrossFadeColor(targetColor, instant ? 0f : _colorBlock.fadeDuration, true, true);
    }
}
