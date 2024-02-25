using UnityEngine;

public class GameAd : MonoBehaviour
{
    private void OnEnable()
    {
        YMoney.Instance.ShowBannerAdv();
    }

    private void OnDisable()
    {
        if(YMoney.Instance != null)
            YMoney.Instance.HideBannerAdv();
    }
}
