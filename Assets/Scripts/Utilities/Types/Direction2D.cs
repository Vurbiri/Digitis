using UnityEngine;

public static class Direction2D
{
    public static Vector2Int Up => Vector2Int.up;
    public static Vector2Int Right => Vector2Int.right;
    public static Vector2Int Down => Vector2Int.down;
    public static Vector2Int Left => Vector2Int.left;

    public static int Count => 4;

    public static Vector2Int[] Cardinal { get; } = new Vector2Int[]
    {
        Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left,
    };

    public static Vector2Int[] Rhomb { get; } = new Vector2Int[]
    {
        Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left,
        Vector2Int.one, Vector2Int.one * -1, new(1 , -1), new(-1 , 1),
        Vector2Int.up*2, Vector2Int.right*2, Vector2Int.down*2, Vector2Int.left*2,
    };
}
