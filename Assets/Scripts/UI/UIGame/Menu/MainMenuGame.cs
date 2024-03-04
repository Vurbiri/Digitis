using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuGame : MainMenu
{
    [Scene, Space]
    [SerializeField] private int _sceneMenu = 2;

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
