using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CustomButtonTargetGraphic))]
public class CustomButton : Button
{
    private CustomButtonTargetGraphic _targetGraphic;

    protected override void Awake()
    {
        base.Awake();

        _targetGraphic = GetComponent<CustomButtonTargetGraphic>();
        _targetGraphic.Initialize(interactable);

        transition = Transition.None;
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        if (!gameObject.activeInHierarchy)
            return;

        switch (state)
        {
            case SelectionState.Normal:
                _targetGraphic.SetNormalState();
                break;
            case SelectionState.Highlighted:
                _targetGraphic.SetHighlightedState();
                break;
            case SelectionState.Pressed:
                _targetGraphic.SetPressedState();
                break;
            case SelectionState.Selected:
                _targetGraphic.SetSelectedState();
                break;
            case SelectionState.Disabled:
                _targetGraphic.SetDisabledState();
                break;
            default:
                _targetGraphic.SetDisabledState();
                break;
        }
    }
}
