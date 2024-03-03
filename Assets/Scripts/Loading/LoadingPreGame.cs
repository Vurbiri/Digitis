using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPreGame : MonoBehaviour
{
    [SerializeField, Scene] private int _nextSceneMobile = 1;
    [SerializeField, Scene] private int _nextSceneDesktop = 1;
    [Space]
    [SerializeField] private Slider _slider;
    [SerializeField] private LogOnPanel _logOnPanel;

    private void Start() => Loading().Forget();

    private async UniTaskVoid Loading()
    {
        Message.Log("Start LoadingPreGame");

        LoadScene loadScene = null;

        YandexSDK ysdk = YandexSDK.InstanceF;
        Localization localization = Localization.InstanceF;
        SettingsGame settings = SettingsGame.InstanceF;

        if (!localization.Initialize())
            Message.Error("Error loading Localization!");

        ProgressLoad(0.1f);

        if (!await InitializeYSDK())
            Message.Log("YandexSDK - initialization error!");

        ProgressLoad(0.2f);

        settings.SetPlatform();
        Banners.InstanceF.Initialize();

        ProgressLoad(0.22f);

        StartLoadScene();
        await CreateStorages();
        
        if (!ysdk.IsLogOn)
        {
            if (await _logOnPanel.TryLogOn())
                await CreateStorages();
        }

        Message.Log("End LoadingPreGame");
        loadScene.End();

        #region Local Functions
        async UniTask<bool> InitializeYSDK()
        {
            if (!await ysdk.InitYsdk())
                return false;

            if (!await ysdk.InitPlayer())
                Message.Log("Player - initialization error!");

            if (!await ysdk.InitLeaderboards())
                Message.Log("Leaderboards - initialization error!");

            return true;
        }
        void StartLoadScene()
        {
            loadScene = new(settings.IsDesktop ? _nextSceneDesktop : _nextSceneMobile, _slider, true);
            loadScene.Start();

            ProgressLoad(0.27f);
        }
        async UniTask CreateStorages(string key = null)
        {
            if (!Storage.StoragesCreate())
                Message.Banner(localization.GetText("ErrorStorage"), MessageType.Error, 7000);
            
            ProgressLoad(0.35f);

            settings.IsFirstStart = !await InitializeStorages();

            ProgressLoad(0.42f);

            #region Local Functions
            async UniTask<bool> InitializeStorages()
            {
                bool isLoad = await Storage.Initialize(key);
            
                if (isLoad)
                    Message.Log("Storage initialize");
                else
                    Message.Log("Storage not initialize");

                return Load(isLoad);

                #region Local Functions
                bool Load(bool load)
                {
                    bool result = false;

                    result = settings.Initialize(load) || result;
                    return DataGame.InstanceF.Initialize(load) || result;
                }
                #endregion
            }
            #endregion
        }

        void ProgressLoad(float value)
        {
            if (loadScene != null)
                loadScene.SetProgress(value);
            else
                _slider.value = value;
        }
        #endregion
    }

    //private void OnDisable() => YandexSDK.Instance.LoadingAPI_Ready();
}
