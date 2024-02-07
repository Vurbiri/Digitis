using UnityEngine;

public static class Message
{
    public static void Log(string msg)
    {
        UtilityJS.Log(msg);
    }

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
        Banners.Instance.Message(string.Format(Localization.Instance.GetText(key), value), type, time, isThrough);
    }
    public static void BannersClear() => Banners.Instance.Clear();

    public static void Saving(string goodMSG, bool isSaving)
    {
        if (isSaving)
            BannerKey(goodMSG, time: 2500);
        else
            BannerKey("ErrorSave", MessageType.Error, 4500);
    }

    public static void Repair(object hp, bool msg = false, int time = 5000)
    {
        if (msg)
            BannerKey("AdReward", time: time);

        BannerKeyFormat("Repair", hp, time: time);
    }
}

