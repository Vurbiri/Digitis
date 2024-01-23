using Cysharp.Threading.Tasks;
using System;

public class EmptyStorage : SaveLoadJsonTo
{
    public override bool IsValid => true;

    public override async UniTask<bool> Initialize(string key)
    {
        await UniTask.Delay(0, true);
        return false;
    }
    public override (bool result, T value) Load<T>(string key) => (false, default);
    public override void Save(string key, object data, bool isSaveHard, Action<bool> callback) => callback?.Invoke(false);
}
