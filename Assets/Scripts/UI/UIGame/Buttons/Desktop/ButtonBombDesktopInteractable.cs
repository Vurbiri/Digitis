using UnityEngine;

[RequireComponent(typeof(ButtonClickHotkey))]
public class ButtonBombDesktopInteractable : AButtonBombInteractable
{
    ButtonClickHotkeyBomb _thisButtonClickHotkeyBomb;

    protected override void SetStatus(int countBomb)
    {
        if(_thisButtonClickHotkeyBomb == null)
            _thisButtonClickHotkeyBomb = _thisButtonClick as ButtonClickHotkeyBomb;

        if (countBomb <= 0)
            _thisButtonClickHotkeyBomb.IsActive = false;
        else
            _thisButtonClickHotkeyBomb.IsActive = true;
    }
}
