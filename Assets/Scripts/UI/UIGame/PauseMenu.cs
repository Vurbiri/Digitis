using System;

public class PauseMenu : SettingsMenu
{
    public event Action EventClose;

    protected override void OnDisable()
    {
        EventClose?.Invoke();
        base.OnDisable();
    }

    public void SetActive(bool value) => gameObject.SetActive(value);
}
