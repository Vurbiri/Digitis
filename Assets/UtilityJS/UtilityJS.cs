using System.Runtime.InteropServices;

public static class UtilityJS
{
#if UNITY_EDITOR
	public static bool IsMobile => false;
#else
	public static bool IsMobile => IsMobileUnityJS();
#endif

	public static void Log(string message) => LogJS(message);
	public static void Error(string message) => ErrorJS(message);

	public static bool SetStorage(string key, string data) => SetStorageJS(key, data);
	public static string GetStorage(string key) => GetStorageJS(key);
	public static bool IsStorage() => IsStorageJS();

	public static bool SetCookies(string key, string data) => SetCookiesJS(key, data);
	public static string GetCookies(string key) => GetCookiesJS(key);
	public static bool IsCookies() => IsCookiesJS();


	[DllImport("__Internal")] internal static extern bool IsMobileUnityJS();
	[DllImport("__Internal")] internal static extern void LogJS(string msg);
	[DllImport("__Internal")] internal static extern void ErrorJS(string msg);
	[DllImport("__Internal")] internal static extern bool SetStorageJS(string key, string data);
	[DllImport("__Internal")] internal static extern string GetStorageJS(string key);
	[DllImport("__Internal")] internal static extern bool IsStorageJS();
	[DllImport("__Internal")] internal static extern bool SetCookiesJS(string key, string data);
	[DllImport("__Internal")] internal static extern string GetCookiesJS(string key);
	[DllImport("__Internal")] internal static extern bool IsCookiesJS();
}
