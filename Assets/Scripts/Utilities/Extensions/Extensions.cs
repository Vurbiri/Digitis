using System.Collections.Generic;

public static class Extensions
{
    public static bool RemoveByValue<TKey, TValue>(this Dictionary<TKey, TValue> self, TValue value)
    {
        TKey key = default;
        bool isFind = false;
        foreach(var kvp in self)
        {
            if (value.Equals(kvp.Value))
            { 
                isFind = true;
                key = kvp.Key;
                break;
            }
        }
        if (isFind)
            isFind = self.Remove(key);

        return isFind;
    }
}
