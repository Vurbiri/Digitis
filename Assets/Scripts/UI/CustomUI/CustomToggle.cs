using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CustomTargetGraphic))]
public class CustomToggle : Toggle
{
    private CustomTargetGraphic _targetGraphic;

    protected override void Awake()
    {
        base.Awake();

        _targetGraphic = GetComponent<CustomTargetGraphic>();
        _targetGraphic.Initialize(interactable);

        transition = Transition.None;

        onValueChanged.AddListener(OnSelect);
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        if (!gameObject.activeInHierarchy || _targetGraphic == null)
            return;

        switch (state)
        {
            case SelectionState.Normal:
                OnSelect(isOn);
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
    private void OnSelect(bool value)
    {
        if (isOn)
            _targetGraphic.SetSelectedState();
        else
            _targetGraphic.SetNormalState();
    }
}
