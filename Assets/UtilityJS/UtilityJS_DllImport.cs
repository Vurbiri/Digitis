using System.Runtime.InteropServices;

public partial class UtilityJS
{
    [DllImport("__Internal")]
    private static extern bool IsMobileUnityJS();
    [DllImport("__Internal")]
    private static extern void LogJS(string msg);
    [DllImport("__Internal")]
    private static extern void ErrorJS(string msg);
    [DllImport("__Internal")]
    private static extern bool SetStorageJS(string key, string data);
    [DllImport("__Internal")]
    private static extern string GetStorageJS(string key);
    [DllImport("__Internal")]
    private static extern bool IsStorageJS();
    [DllImport("__Internal")]
    private static extern bool SetCookiesJS(string key, string data);
    [DllImport("__Internal")]
    private static extern string GetCookiesJS(string key);
    [DllImport("__Internal")]
    private static extern bool IsCookiesJS();
    [DllImport("__Internal")]
    private static extern void IsAccelerometerJS();
}
