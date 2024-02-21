using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MenuNavigation
{
    [Space]
    [SerializeField] private Game _game;
    [SerializeField] private AInputController _inputController;
    [Space]
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private GameObject _panelPause;
    [Space]
    [SerializeField] private GameObject _panelLeaderboard;
    [SerializeField] private LeaderboardUI _leaderboardUI;
    [SerializeField] private GameObject _area;
    [SerializeField] private GameObject _next;
    [Scene, Space]
    [SerializeField] private int _sceneMenu = 1;

    private SettingsGame _settings;

    public event Action EventClose;

    protected override void Awake()
    {
        base.Awake();
        _settings = SettingsGame.InstanceF;

        _inputController.EventPause += () => gameObject.SetActive(true);
        _game.EventLeaderboard += OnLeaderboard;

        _pauseUI.SetActive(true);
        _panelPause.SetActive(true);
        _panelLeaderboard.SetActive(false);

        gameObject.SetActive(false);

        void OnLeaderboard()
        {
            _panelPause.SetActive(false);
            _area.SetActive(false);
            _next.SetActive(false);
            _panelLeaderboard.SetActive(true); 
            _leaderboardUI.TryReward().Forget(); 
            gameObject.SetActive(true);
        }
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
