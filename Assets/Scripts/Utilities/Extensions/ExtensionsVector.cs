using UnityEngine;

public static class ExtensionsVector
{
    public static Vector2Int ToVector2Int(this Vector3 self) => new(Mathf.RoundToInt(self.x), Mathf.RoundToInt(self.y));
    public static Vector2Int ToVector2Int(this Vector2 self) => new(Mathf.RoundToInt(self.x), Mathf.RoundToInt(self.y));

}
