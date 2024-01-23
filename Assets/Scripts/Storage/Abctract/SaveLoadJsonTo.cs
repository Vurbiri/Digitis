using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

public abstract class SaveLoadJsonTo
{

    protected Dictionary<string, string> _saved = null;
    protected bool _dictModified = false;

    public abstract bool IsValid { get; }

    public abstract UniTask<bool> Initialize(string key);

    public virtual (bool result, T value) Load<T>(string key)
    {
        if ( _saved.TryGetValue(key, out string json))
            return Deserialize<T>(json);

        return (false, default);
    }

    public abstract void Save(string key, object data, bool isSaveHard, Action<bool> callback);
    protected virtual bool SaveSoft(string key, object data)
    {
        try
        {
            string json = Serialize(data);
            _saved.TryGetValue(key, out string saveJson);
            if (saveJson == null || saveJson != json)
            {
                _saved[key] = json;
                _dictModified = true;
            }
            return true;
        }
        catch (Exception ex)
        {
            Message.Log(ex.Message);
        }

        return false;
    }

    protected string Serialize(object obj) => Storage.Serialize(obj);
    protected (bool result, T value) Deserialize<T>(string json) => Storage.Deserialize<T>(json);

}
