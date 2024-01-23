using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Networking.UnityWebRequest;

public static class Storage
{
    private const string keyGlobalSave = "DTS";
    
    private static SaveLoadJsonTo service;

    public static Type TypeStorage => service?.GetType();

    public static bool Create<T>() where T : SaveLoadJsonTo, new()
    {
        if (typeof(T) == TypeStorage)
            return true; 

        service = new T();
        return service.IsValid;
    }
    public static bool StoragesCreate()
    {
        if (Create<JsonToYandex>())
            return true;

        if (Create<JsonToLocalStorage>())
            return true;

        if (Create<JsonToCookies>())
            return true;

        Create<EmptyStorage>();
        return false;
    }
    public static UniTask<bool> Initialize(string key = null)
    {
        return service.Initialize(string.IsNullOrEmpty(key) ? keyGlobalSave : key);
    }

    public static void Save(string key, object data, bool isSaveHard = true, Action<bool> callback = null) => service.Save(key, data, isSaveHard, callback);
    public static UniTask<bool> SaveAsync(string key, object data, bool isSaveHard = true)
    {
        UniTaskCompletionSource<bool> taskSave = new();
        service.Save(key, data, isSaveHard, (b) => taskSave.TrySetResult(b));
        return taskSave.Task;
    }
    public static (bool result, T value) Load<T>(string key) => service.Load<T>(key);

    public static (bool result, T value) Deserialize<T>(string json)
    {
        (bool, T) result = (false, default);

        try
        {
            result.Item2 = JsonConvert.DeserializeObject<T>(json);
            result.Item1 = result.Item2 != null;
        }
        catch (Exception ex)
        {
            Message.Log(ex.Message);
        }

        return result;
    }
    public static string Serialize(object obj) => JsonConvert.SerializeObject(obj);

    public static async UniTask<(bool result, Texture texture)> TryLoadTextureWeb(string url)
    {
        if (string.IsNullOrEmpty(url))
            return (false, null);

        using var request = UnityWebRequestTexture.GetTexture(url);
        await request.SendWebRequest();

        if (request.result != Result.Success)
            return (false, null);

        return (true, ((DownloadHandlerTexture)request.downloadHandler).texture);
    }
}
