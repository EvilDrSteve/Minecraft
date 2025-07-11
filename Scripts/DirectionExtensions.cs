using System;
using UnityEngine;

public static class DirectionExtensions
{
    public static Vector3Int GetVector(this Direction direction)
    {
        return direction switch
        {
            Direction.Forward => Vector3Int.forward,
            Direction.Backward => Vector3Int.back,
            Direction.Right => Vector3Int.right,
            Direction.Left => Vector3Int.left,
            Direction.Up => Vector3Int.up,
            Direction.Down => Vector3Int.down,
            _ => throw new Exception("Invalid input direction")
        };
    }
}