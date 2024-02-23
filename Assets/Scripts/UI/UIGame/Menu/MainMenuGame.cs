using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuGame : MainMenu
{
    [Scene, Space]
    [SerializeField] private int _sceneMenu = 2;

    public void OnToMenu()
    {
        SceneManager.LoadSceneAsync(_sceneMenu);
        Time.timeScale = 1.0f;
    }
}
