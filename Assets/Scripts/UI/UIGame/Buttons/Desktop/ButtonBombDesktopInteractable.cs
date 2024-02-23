using UnityEngine;

[RequireComponent(typeof(ButtonClickHotkey))]
public class ButtonBombDesktopInteractable : AButtonBombInteractable
{
    protected override void SetStatus(int countBomb)
    {
        ButtonClickHotkeyBomb thisButton = _thisButtonClick as ButtonClickHotkeyBomb;

        if (countBomb <= 0)
        {
            if (thisButton.IsActive)
                thisButton.IsActive = false;
        }
        else
        {
            if (!thisButton.IsActive)
                thisButton.IsActive = true;
        }
    }

}
