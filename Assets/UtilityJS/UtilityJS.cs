public static partial class UtilityJS
{
#if !UNITY_EDITOR
    public static bool IsMobile => IsMobileUnityJS();

    public static void Log(string message) => LogJS(message);
    public static void Error(string message) => ErrorJS(message);

    public static bool SetStorage(string key, string data) => SetStorageJS(key, data);
    public static string GetStorage(string key) => GetStorageJS(key);
    public static bool IsStorage() => IsStorageJS();

    public static bool SetCookies(string key, string data) => SetCookiesJS(key, data);
    public static string GetCookies(string key) => GetCookiesJS(key);
    public static bool IsCookies() => IsCookiesJS();
#endif
}
