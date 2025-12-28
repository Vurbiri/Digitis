#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

public class JsonToPlayerPrefs : ASaveLoadJsonTo
{
    private string _key;

    public override bool IsValid => true;

    public override bool Initialize(string key)
    {
        _key = key;

        if (PlayerPrefs.HasKey(_key))
        {
            string json = PlayerPrefs.GetString(_key);

            if (json != null)
            {
                var d = Deserialize<Dictionary<string, string>>(json);

                if (d.Result)
                {
                    _saved = d.Value;
                    return true;
                }
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
            PlayerPrefs.SetString(_key, json);
            PlayerPrefs.Save();
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
#endif