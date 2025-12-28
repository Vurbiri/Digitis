#if UNITY_EDITOR
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonToFile : ASaveLoadJsonTo
{
    private string _path;

    public override bool IsValid => true;

    public override bool Initialize(string fileName)
    {
        _path = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(_path))
        {
            string json;

            using (StreamReader sr = new(_path))
            {
                json = sr.ReadToEnd();
            }

            if (!string.IsNullOrEmpty(json))
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
            using StreamWriter sw = new(_path);
            await sw.WriteAsync(json);
            _dictModified = false;
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