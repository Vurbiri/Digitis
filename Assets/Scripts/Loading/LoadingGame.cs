using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingGame : MonoBehaviour
{
    [SerializeField, Scene] private int _gameSceneMobile = 4;
    [SerializeField, Scene] private int _gameSceneDesktop = 5;

    private void Start()
    {
        SceneManager.LoadSceneAsync(SettingsGame.Instance.IsDesktop ? _gameSceneDesktop : _gameSceneMobile);
    }
}
