using Cysharp.Threading.Tasks;
using System;

public class EmptyStorage : ASaveLoadJsonTo
{
    public override bool IsValid => true;

    public override async UniTask<bool> Initialize(string key)
    {
        await UniTask.Delay(0, true);
        return false;
    }
    public override ReturnValue<T> Load<T>(string key) => new();
    public override void Save(string key, object data, bool isSaveHard, Action<bool> callback) => callback?.Invoke(false);
}
