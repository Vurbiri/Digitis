using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPreGame : MonoBehaviour
{
    [Scene]
    [SerializeField] protected int _nextSceneMobile = 1;
    [Scene]
    [SerializeField] protected int _nextSceneDesktop = 1;
    [Space]
    [SerializeField] protected Slider _slider;
    //[SerializeField] private LogOnWindow _logOnWindow;

    private void Start() => Loading().Forget();

    private async UniTaskVoid Loading()
    {
        Message.Log("Start LoadingPreGame");

        LoadScene loadScene = null;

        YandexSDK ysdk = YandexSDK.InstanceF;
        Localization localization = Localization.InstanceF;

        if (!localization.Initialize())
        {
            Message.Banner("Error loading Localization!", MessageType.FatalError);
            return;
        }

        ProgressLoad(0.1f);

        if (!await InitializeYSDK())
            Message.Log("YandexSDK - initialization error!");

        ProgressLoad(0.2f);

        bool isDesktop;
        StartLoadScene();

        ProgressLoad(0.3f);

        await CreateStorages();

        //if (!ysdk.IsLogOn)
        //{
        //    _slider.gameObject.SetActive(false);
        //    if (await _logOnWindow.TryLogOn())
        //        await CreateStorages();
        //    _slider.gameObject.SetActive(true);
        //}

        ProgressLoad(0.5f);

        Message.Log("End LoadingPreGame");
        loadScene.End();

        #region Local Functions
        void StartLoadScene()
        {
            if (ysdk.IsPlayer)
                isDesktop = ysdk.IsDesktop;
            else
                isDesktop = !UtilityJS.IsMobile;

            if (isDesktop)
                loadScene = new(_nextSceneDesktop, _slider, true);
            else
                loadScene = new(_nextSceneMobile, _slider, true);

            loadScene.Start();
        }

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
        async UniTask CreateStorages(string key = null)
        {
            if (!Storage.StoragesCreate())
                Message.Banner(localization.GetText("ErrorStorage"), MessageType.Error, 7000);
            
            ProgressLoad(0.35f);

            Settings.Instance.IsFirstStart = !await InitializeStorages();
            
            ProgressLoad(0.4f);

            #region Local Functions
            async UniTask<bool> InitializeStorages()
            {
                bool isLoad = await Storage.Initialize(key);
            
                if (isLoad)
                    Message.Log("Storage Initialize");
                else
                    Message.Log("Storage Not Initialize");

                return Load(isLoad);

                #region Local Functions
                bool Load(bool b)
                {
                    bool result = false;

                    result = Settings.Instance.Initialize(b, isDesktop) || result;
                    return GameData.Instance.Initialize(b) || result;
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
