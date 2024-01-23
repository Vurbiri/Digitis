using Cysharp.Threading.Tasks;
using System;

public partial class YandexSDK
{
    private UniTaskCompletionSource<bool> taskEndInitYsdk;
    private UniTaskCompletionSource<bool> taskEndInitPlayer;
    private UniTaskCompletionSource<bool> taskEndLogOn;
    private UniTaskCompletionSource<bool> taskEndInitLeaderboards;
    private UniTaskCompletionSource<bool> taskEndSetScore;
    private UniTaskCompletionSource<string> taskEndGetPlayerResult;
    private UniTaskCompletionSource<string> taskEndGetLeaderboard;
    private UniTaskCompletionSource<bool> taskEndSave;
    private UniTaskCompletionSource<string> taskEndLoad;
    private UniTaskCompletionSource<bool> taskEndCanReview;
    private UniTaskCompletionSource<bool> taskEndRequestReview;
    private UniTaskCompletionSource<bool> taskEndCanShortcut;
    private UniTaskCompletionSource<bool> taskEndCreateShortcut;

    public void OnEndInitYsdk(int result) => taskEndInitYsdk?.TrySetResult(Convert.ToBoolean(result));
    public void OnEndInitPlayer(int result) => taskEndInitPlayer?.TrySetResult(Convert.ToBoolean(result));
    public void OnEndLogOn(int result) => taskEndLogOn?.TrySetResult(Convert.ToBoolean(result));
    public void OnEndInitLeaderboards(int result) => taskEndInitLeaderboards?.TrySetResult(Convert.ToBoolean(result));
    public void OnEndSetScore(int result) => taskEndSetScore?.TrySetResult(Convert.ToBoolean(result));
    public void OnEndGetPlayerResult(string value) => taskEndGetPlayerResult?.TrySetResult(value);
    public void OnEndGetLeaderboard(string value) => taskEndGetLeaderboard?.TrySetResult(value);
    public void OnEndSave(int result) => taskEndSave?.TrySetResult(Convert.ToBoolean(result));
    public void OnEndLoad(string value) => taskEndLoad?.TrySetResult(value);
    public void OnEndCanReview(int result) => taskEndCanReview?.TrySetResult(Convert.ToBoolean(result));
    public void OnEndRequestReview(int result) => taskEndRequestReview?.TrySetResult(Convert.ToBoolean(result));
    public void OnEndCanShortcut(int result) => taskEndCanShortcut?.TrySetResult(Convert.ToBoolean(result));
    public void OnEndCreateShortcut(int result) => taskEndCreateShortcut?.TrySetResult(Convert.ToBoolean(result));
}
