using Cysharp.Threading.Tasks;
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
    [Space]
    [SerializeField] private GameObject _panelLeaderboard;
    [SerializeField] private LeaderboardUI _leaderboardUI;
    [Space]
    [SerializeField] private Title _title;
    [SerializeField] private string _key = "Digitis";
    [Scene, Space]
    [SerializeField] private int _sceneMenu = 1;

    private SettingsGame _settings;

    protected override void Awake()
    {
        base.Awake();
        _settings = SettingsGame.Instance;

        _inputController.EventPause += () => gameObject.SetActive(true);
        _game.EventLeaderboard += OnLeaderboard;

        _panelPause.SetActive(true);
        _panelLeaderboard.SetActive(false);

        gameObject.SetActive(false);

        void OnLeaderboard()
        {
            _leaderboardUI.TryReward().Forget();

            _title.Key = _key;
            _panelPause.SetActive(false);
            _panelLeaderboard.SetActive(true); 
            gameObject.SetActive(true);
        }
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
        Time.timeScale = 1.0f;
    }
}
