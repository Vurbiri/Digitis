using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPreGame : MonoBehaviour
{
    [SerializeField] protected int _nextScene = 1;
    [Space]
    [SerializeField] protected Slider _slider;
    //[SerializeField] private LogOnWindow _logOnWindow;

    private void Start() => Loading().Forget();

    private async UniTaskVoid Loading()
    {
        Message.Log("Start LoadingPreGame");

        float progress = 0f;
        LoadScene loadScene = new(_nextScene);
        loadScene.Start(OnProgressLoad);

        YandexSDK ysdk = YandexSDK.InstanceF;
        Localization localization = Localization.InstanceF;
        YMoney ym = YMoney.InstanceF;

        ProgressInitialize(0.1f);

        if (!localization.Initialize())
        {
            Message.Banner("Error loading Localization!", MessageType.FatalError);
            return;
        }

        ProgressInitialize(0.2f);

        if (!await InitializeYSDK())
            Message.Log("YandexSDK - initialization error!");

        await CreateStorages();

        //if (!ysdk.IsLogOn)
        //{
        //    _slider.gameObject.SetActive(false);
        //    if (await _logOnWindow.TryLogOn())
        //        await CreateStorages();
        //    _slider.gameObject.SetActive(true);
        //}

        ProgressInitialize(0.5f);

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

            ProgressInitialize(0.3f);
            return true;
        }
        async UniTask CreateStorages(string key = null)
        {
            if (!Storage.StoragesCreate())
                Message.Banner(localization.GetText("ErrorStorage"), MessageType.Error, 7000);
            
            ProgressInitialize(0.35f);
            
            ym.IsFirstStart = !await InitializeStorages();
            
            ProgressInitialize(0.4f);

            //============== local func ===============================
            async UniTask<bool> InitializeStorages()
            {
                bool isLoad = await Storage.Initialize(key);
            
                if (isLoad)
                    Message.Log("Storage Initialize");
                else
                    Message.Log("Storage Not Initialize");

                return Load(isLoad);

                //============== local func ===============================
                bool Load(bool b)
                {
                    bool result = false;

                    return result = SettingsGame.Instance.Initialize(b) || result;
                    //result = PlayerStates.Instance.Initialize(b) || result;
                    //return GameData.Instance.Initialize(b) || result;
                }
            }
        }
        void OnProgressLoad(float loading)
        {
            _slider.value = loading * 0.5f + progress;
        }
        void ProgressInitialize(float value)
        {
            progress = value;
            _slider.value = loadScene.Progress * 0.5f + progress;
        }
        #endregion
    }

    //private void OnDisable() => YandexSDK.Instance.LoadingAPI_Ready();

}
