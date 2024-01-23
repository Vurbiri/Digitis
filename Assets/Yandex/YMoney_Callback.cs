using Cysharp.Threading.Tasks;
using System;

public partial class YMoney
{
    private UniTaskCompletionSource<bool> taskEndShowFullscreenAdv;
    private UniTaskCompletionSource<bool> taskRewardRewardedVideo;
    private UniTaskCompletionSource<bool> taskCloseRewardedVideo;

    public void OnEndShowFullscreenAdv(int result) => taskEndShowFullscreenAdv?.TrySetResult(Convert.ToBoolean(result));
    public void OnRewardRewardedVideo(int result) => taskRewardRewardedVideo?.TrySetResult(Convert.ToBoolean(result));
    public void OnCloseRewardedVideo(int result) => taskCloseRewardedVideo?.TrySetResult(Convert.ToBoolean(result));
}
