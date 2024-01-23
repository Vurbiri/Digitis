using System;
using System.IO;
using UnityEngine;

public static class StorageResources
{
    public static (bool result, T value) LoadFromJson<T>(string path)
    {
        string json = Resources.Load<TextAsset>(path).text;
        return Storage.Deserialize<T>(json);
    }


#if UNITY_EDITOR
    private const string _pathToResources = "e:/Projects Unity/ProbeA/Assets/Resources/";

    public static void Save(string fileName, object data)
    {
        string path = Path.Combine(_pathToResources, fileName);
        try
        {
            string json = Storage.Serialize(data);
            using StreamWriter sw = new(path);
            sw.Write(json);

        }
        catch (Exception ex)
        {
            Message.Log(ex.Message);
        }
    }
#endif
}
