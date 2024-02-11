using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CustomButtonTargetGraphic))]
public class CustomButton : Button
{

    private CustomButtonTargetGraphic _targetGraphic;

    protected new void Awake()
    {
        _targetGraphic = GetComponent<CustomButtonTargetGraphic>();
        transition = Transition.None;
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        if (!gameObject.activeInHierarchy)
            return;

        switch (state)
        {
            case SelectionState.Normal:
                _targetGraphic.SetNormalState(instant);
                break;
            case SelectionState.Highlighted:
                _targetGraphic.SetHighlightedState(instant);
                break;
            case SelectionState.Pressed:
                _targetGraphic.SetPressedState(instant);
                break;
            case SelectionState.Selected:
                _targetGraphic.SetSelectedState(instant);
                break;
            case SelectionState.Disabled:
                _targetGraphic.SetDisabledState(instant);
                break;
            default:
                _targetGraphic.SetDisabledState(instant);
                break;
        }
    }
}
