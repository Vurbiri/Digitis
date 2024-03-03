using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuGame : MainMenu
{
    [Space]
    [SerializeField] private GameObject _firstOpenMenu;
    [Scene, Space]
    [SerializeField] private int _sceneMenu = 2;

    protected override void OnEnable()
    {
        base.OnEnable();
        _firstOpenMenu.SetActive(true);
    }

    protected override async UniTask ButtonInitialize()
    {
        await base.ButtonInitialize();

        _leaderboard.interactable = _leaderboard.interactable && !DataGame.Instance.IsInfinityMode;
    }

    public void OnToMenu()
    {
        SceneManager.LoadSceneAsync(_sceneMenu);
    }
}
