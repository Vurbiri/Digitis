using System.Collections.Generic;
using UnityEngine;

public class SubShape
{
    public SubShape NextSubShape { get; }
    public Vector2Int[] DeltaPositions { get; }
    public HashSet<Vector2Int> CollisionRotation { get; }

    public SubShape(Vector2Int[] blocksPositions, int maxIndexesY, int countInstance, SubShape nextSubShape = null)
    {
        bool isEnd = --countInstance == 0;
        if (isEnd)
            NextSubShape = nextSubShape;

        int count = blocksPositions.Length;
        Vector2Int[] nextBlocksPositions = new Vector2Int[count];
        DeltaPositions = new Vector2Int[count];
        CollisionRotation = new();
        Vector2Int min, max, curr, next;

        for (int i = 0; i < count; i++) 
        {
            curr = blocksPositions[i];
            next = new(maxIndexesY - curr.y, curr.x);
            
            nextBlocksPositions[i] = next;
            DeltaPositions[i] = next - curr;

            min = Vector2Int.Min(curr, next);
            max = Vector2Int.Max(curr, next);
            for( int x = min.x; x <= max.x; x++ ) 
            { 
                for( int y = min.y; y <= max.y; y++ ) 
                {
                    next.x = x; next.y = y;
                    if( next != curr)
                        CollisionRotation.Add(next);
                }
            }
        }

        CollisionRotation.TrimExcess();
        if (!isEnd)
            NextSubShape = new(nextBlocksPositions, maxIndexesY, countInstance, nextSubShape ?? this);
    }

}
