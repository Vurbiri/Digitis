using System;

public class EmptyStorage : ASaveLoadJsonTo
{
    public override bool IsValid => true;

    public override bool Initialize(string key) => false;

    public override Return<T> Load<T>(string key) => Return<T>.Empty;
    public override void Save(string key, object data, bool isSaveHard, Action<bool> callback) => callback?.Invoke(false);
}
