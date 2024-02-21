using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CustomTargetGraphic))]
public class CustomToggle : Toggle
{
    private CustomTargetGraphic _thisTargetGraphic;

    protected override void Awake()
    {
        base.Awake();

        _thisTargetGraphic = GetComponent<CustomTargetGraphic>();
        _thisTargetGraphic.Initialize(interactable);

        transition = Transition.None;

        onValueChanged.AddListener(OnSelect);
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        if (!gameObject.activeInHierarchy || _thisTargetGraphic == null)
            return;

        switch (state)
        {
            case SelectionState.Normal:
                OnSelect(isOn);
                break;
            case SelectionState.Highlighted:
                _thisTargetGraphic.SetHighlightedState();
                break;
            case SelectionState.Pressed:
                _thisTargetGraphic.SetPressedState();
                break;
            case SelectionState.Selected:
                _thisTargetGraphic.SetSelectedState();
                break;
            case SelectionState.Disabled:
                _thisTargetGraphic.SetDisabledState();
                break;
            default:
                _thisTargetGraphic.SetDisabledState();
                break;
        }
    }
    private void OnSelect(bool value)
    {
        if (isOn)
            _thisTargetGraphic.SetSelectedState();
        else
            _thisTargetGraphic.SetNormalState();
    }
}
