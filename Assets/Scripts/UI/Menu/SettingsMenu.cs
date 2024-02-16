public class SettingsMenu : MenuNavigation
{
    protected SettingsGame _settings;
    private bool isSave;


    protected override void Awake()
    {
        base.Awake();
        _settings = SettingsGame.InstanceF;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        isSave = false;
    }
    protected virtual void OnDisable() 
    {
        if (SettingsGame.Instance == null)
            return;
        
        if (isSave)
            _settings.Save(true, (b) => Message.Saving("GoodSaveSettings", b));
        else
            _settings.Cancel();
    }

    public void OnOk()
    {
        isSave = true;
    }
}
