using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuMobile : MenuNavigation
{
    [Space]
    [SerializeField] private Game _game;
    [SerializeField] private InputMobileController _inputController;
    [Space]
    [SerializeField] private GameObject _panelPause;
    [Scene, Space]
    [SerializeField] private int _sceneMenu = 1;

    private SettingsGame _settings;

    protected override void Awake()
    {
        base.Awake();
        _settings = SettingsGame.Instance;

        _inputController.EventPause += () => gameObject.SetActive(true);

        _panelPause.SetActive(true);

        gameObject.SetActive(false);
    }

    public void OnOk()
    {
        _settings.Save(true, (b) => Message.Saving("GoodSaveSettings", b));
    }

    public void OnCancel()
    {
        _settings.Cancel();
    }

    public void OnToMenu()
    {
        SceneManager.LoadSceneAsync(_sceneMenu);
    }
}
