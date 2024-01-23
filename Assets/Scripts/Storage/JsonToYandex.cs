using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

public class JsonToYandex : SaveLoadJsonTo
{
    private string _key;
    private YandexSDK YSDK => YandexSDK.Instance;

    public override bool IsValid => YSDK.IsLogOn;

    public async override UniTask<bool> Initialize(string key)
    {
        _key = key;
        string json;

        try
        {
            json = await YSDK.Load(_key);
        }
        catch (Exception ex)
        {
            json = null;
            Message.Log(ex.Message);
        }

        if (!string.IsNullOrEmpty(json))
        {
            var (result, value) = Deserialize<Dictionary<string, string>>(json);

            if (result)
            {
                _saved = value;
                return true;
            }
        }

        _saved = new();
        return false;
    }

    public override void Save(string key, object data, bool isSaveHard, Action<bool> callback)
    {
        bool result;
        if (!((result = SaveSoft(key, data)) && isSaveHard && _dictModified))
        {
            callback?.Invoke(result);
            return;
        }

        SaveToFileAsync(callback).Forget();
    }

    public async UniTaskVoid SaveToFileAsync(Action<bool> callback)
    {
        bool result = true;
        try
        {
            string json = Serialize(_saved);
            result = await YSDK.Save(_key, json);
            _dictModified = !result;
        }
        catch (Exception ex)
        {
            result = false;
            Message.Log(ex.Message);
        }
        finally
        {
            callback?.Invoke(result);
        }
    }
}
