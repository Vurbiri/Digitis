using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MenuNavigation
{
    [Space]
    [SerializeField] private Game _game;
    [SerializeField] private AGameController _gameController;
    [Space]
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private GameObject _panelPause;
    [SerializeField] private GameObject _panelGameOver;
    [Scene, Space]
    [SerializeField] private int _sceneMenu = 1;

    private SettingsGame _settings;

    public event Action EventClose;

    protected override void Awake()
    {
        base.Awake();
        _settings = SettingsGame.InstanceF;

        _gameController.EventPause += () => gameObject.SetActive(true);

        _pauseUI.SetActive(true);
        _panelPause.SetActive(true);
        _panelGameOver.SetActive(false);

        gameObject.SetActive(false);
    }

    public void OnOk()
    {
        _settings.Save(true, (b) => Message.Saving("GoodSaveSettings", b));
        EventClose?.Invoke();
    }

    public void OnCancel()
    {
        _settings.Cancel();
        EventClose?.Invoke();
    }

    public void OnToMenu()
    {
        SceneManager.LoadSceneAsync(_sceneMenu);
        Time.timeScale = 1.0f;
    }
}
