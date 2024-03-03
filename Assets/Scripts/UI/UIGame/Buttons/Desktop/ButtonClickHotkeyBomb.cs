using Newtonsoft.Json.Bson;
using UnityEngine;

public class ButtonClickHotkeyBomb : ButtonClickHotkey
{
    [Space]
    [SerializeField] private bool _isActive = true;

    public bool IsActive
    {
        get => _isActive;
        set { _isActive = value; SetButtonState();}
    }
    public override bool IsInteractable
    {
        get => _isInteractable && _isActive;
        set { _isInteractable = value; SetButtonState(); }
    }

    private void SetButtonState()
    {
        if (_thisTargetGraphic == null)
            return;

        if (IsInteractable)
            _thisTargetGraphic.SetNormalState();
        else
            _thisTargetGraphic.SetDisabledState();
    }
}
