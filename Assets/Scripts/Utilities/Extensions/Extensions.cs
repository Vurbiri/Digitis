using UnityEngine;

public static class Extensions
{
    public static int RandomIndex<T>(this T[] self, int lastIndex = -1)
    {
        int index;
        do
        { 
            index = Random.Range(0, self.Length); 
        } 
        while (self.Length > 1 && index == lastIndex);
        
        return index;
    }
}
