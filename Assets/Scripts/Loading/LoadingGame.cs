using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class LoadingGame : MonoBehaviour
{
    [SerializeField, Scene] private int _gameSceneMobile = 4;
    [SerializeField, Scene] private int _gameSceneDesktop = 5;
    [Space]
    [SerializeField] private Slider _slider;

    private void Start()
    {
        StartAsync().Forget();

        async UniTaskVoid StartAsync()
        {
            SettingsGame settings = SettingsGame.Instance;

            LoadScene _loadScene = new(settings.IsDesktop ? _gameSceneDesktop : _gameSceneMobile, _slider);
            _loadScene.Start();

            if (!settings.IsFirstStart)
                await YMoney.Instance.ShowFullscreenAdv();

            _loadScene.End();
        }
    }
}
