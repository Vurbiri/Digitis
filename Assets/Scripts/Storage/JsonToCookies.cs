using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

public class JsonToCookies : ASaveLoadJsonTo
{
    private string _key;

    public override bool IsValid => UtilityJS.IsCookies();

    public async override UniTask<bool> Initialize(string key)
    {
        _key = key;

        string json;

        await UniTask.Delay(0, true);
        try
        {
            json = UtilityJS.GetCookies(_key);
        }
        catch (Exception ex)
        {
            json = null;
            Message.Log(ex.Message);
        }

        if (!string.IsNullOrEmpty(json))
        {
            ReturnValue<Dictionary<string, string>> d = Deserialize<Dictionary<string, string>>(json);

            if (d.Result)
            {
                _saved = d.Value;
                return true;
            }
        }

        _saved = new();
        return false;
    }

    public override void Save(string key, object data, bool isSaveHard, Action<bool> callback)
    {
        bool result;
        if (!(result = SaveSoft(key, data)) || !isSaveHard)
        {
            callback?.Invoke(result);
            return;
        }

        try
        {
            string json = Serialize(_saved);
            result = UtilityJS.SetCookies(_key, json);

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
