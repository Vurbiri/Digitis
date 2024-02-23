using UnityEngine;

public class ButtonClickHotkeyBomb : ButtonClickHotkey
{
    [Space]
    [SerializeField] private bool _isActive = true;

    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;

            if (_thisTargetGraphic == null)
                return;

            if (IsInteractable)
                _thisTargetGraphic.SetNormalState();
            else
                _thisTargetGraphic.SetDisabledState();
        }
    }
    public override bool IsInteractable
    {
        get => _isInteractable && _isActive;
        set
        {
            if (_isInteractable == value)
                return;

            _isInteractable = value;
            if (IsInteractable)
                _thisTargetGraphic.SetNormalState();
            else
                _thisTargetGraphic.SetDisabledState();
        }
    }
}
