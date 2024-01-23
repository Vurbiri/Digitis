using System.Runtime.InteropServices;

public partial class YandexSDK
{
    [DllImport("__Internal")]
    private static extern bool IsInitializeJS();
    [DllImport("__Internal")]
    private static extern void ReadyJS();
    [DllImport("__Internal")]
    private static extern bool IsPlayerJS();
    [DllImport("__Internal")]
    private static extern bool IsLogOnJS();
    [DllImport("__Internal")]
    private static extern bool IsLeaderboardJS();
    [DllImport("__Internal")]
    private static extern bool IsDesktopJS();
    [DllImport("__Internal")]
    private static extern bool IsMobileJS();
    [DllImport("__Internal")]
    private static extern string GetPlayerNameJS();
    [DllImport("__Internal")]
    private static extern string GetPlayerAvatarURLJS(string size);
    [DllImport("__Internal")]
    private static extern string GetLangJS();
  
    [DllImport("__Internal")]
    private static extern void InitYsdkJS();
    [DllImport("__Internal")]
    private static extern void InitPlayerJS();
    [DllImport("__Internal")]
    private static extern void LogOnJS();
    [DllImport("__Internal")]
    private static extern void InitLeaderboardsJS();
    [DllImport("__Internal")]
    private static extern void GetPlayerResultJS(string lbName);
    [DllImport("__Internal")]
    private static extern void SetScoreJS(string lbName, int score);
    [DllImport("__Internal")]
    private static extern void GetLeaderboardJS(string lbName, int quantityTop, bool includeUser, int quantityAround, string size);
    [DllImport("__Internal")]
    private static extern void SaveJS(string key, string data);
    [DllImport("__Internal")]
    private static extern void LoadJS(string key);
    [DllImport("__Internal")]
    private static extern void CanReviewJS();
    [DllImport("__Internal")]
    private static extern void RequestReviewJS();

    [DllImport("__Internal")]
    private static extern void CanShortcutJS();
    [DllImport("__Internal")]
    private static extern void CreateShortcutJS();
    
}
