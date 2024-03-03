
public static class Message
{
    public static void Log(string msg)=> UtilityJS.Log(msg);
    public static void Error(string msg) => UtilityJS.Error(msg);

    public static void Banner(string message, MessageType type = MessageType.Normal, int time = 5000, bool isThrough = true)
    {
        Banners.Instance.Message(message, type, time, isThrough);
    }
    public static void BannerKey(string key, MessageType type = MessageType.Normal, int time = 5000, bool isThrough = true)
    {
        Banners.Instance.Message(Localization.Instance.GetText(key), type, time, isThrough);
    }
    public static void BannerKeyFormat(string key, object value, MessageType type = MessageType.Normal, int time = 5000, bool isThrough = true)
    {
        Banners.Instance.Message(Localization.Instance.GetTextFormat(key, value), type, time, isThrough);
    }
    public static void BannersClear() => Banners.Instance.Clear();

    public static void Saving(string goodMSG, bool isSaving)
    {
        if (isSaving)
            BannerKey(goodMSG, time: 2000);
        else
            BannerKey("ErrorSave", MessageType.Error, 5500);
    }
}

