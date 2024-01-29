using UnityEngine;

public static class Direction2D
{
    public static Vector2Int Up => All[0];
    public static Vector2Int Right => All[1];
    public static Vector2Int Down => All[2];
    public static Vector2Int Left => All[3];

    public static int Count => 4;

    public static Vector2Int[] All { get; } = new Vector2Int[]
    {
        Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left,
    };
}
