#if UNITY_EDITOR

using System.IO;
using UnityEngine;

public static partial class UtilityJS
{
    public static bool IsMobile => true;

    public static void Log(string message) => Debug.Log(message);
    public static void Error(string message) => Debug.LogError(message);

    public static bool SetStorage(string key, string data)
    {
        using StreamWriter sw = new(Path.Combine(Application.persistentDataPath, key));
        sw?.Write(data);

        return true;
    }
    public static string GetStorage(string key)
    {
        string path = Path.Combine(Application.persistentDataPath, key);
        if (File.Exists(path))
        {
            using StreamReader sr = new(path);
            return sr.ReadToEnd();
        }

        return null;
    }
    public static bool IsStorage() => false;

    public static bool SetCookies(string key, string data)
    {
        PlayerPrefs.SetString(key, data);
        PlayerPrefs.Save();

        return true;
    }
    public static string GetCookies(string key)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetString(key); 

        return null;
    }
    public static bool IsCookies() => true;
}
#endif