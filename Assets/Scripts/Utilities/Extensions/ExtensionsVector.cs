using UnityEngine;

public static class ExtensionsVector
{
    public static Vector2Int ToVector2Int(this Vector3 self) => new(Mathf.RoundToInt(self.x), Mathf.RoundToInt(self.y));
    public static Vector2Int ToVector2Int(this Vector2 self) => new(Mathf.RoundToInt(self.x), Mathf.RoundToInt(self.y));

    public static Vector3 ToVector3(this Vector2Int self) => new(self.x, self.y, 0f);

    public static float RandomRange(this Vector2 self) => Random.Range(self.x, self.y);

}
