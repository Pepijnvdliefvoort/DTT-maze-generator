using System;
using UnityEngine;

public enum Direction {
    LEFT = 0,
    TOP = 1,
    RIGHT = 2,
    BOTTOM = 3
}

public static class DirectionExtensions {
    /// <summary>
    /// Converts the given direction to a vector to use in world-space
    /// </summary>
    /// <param name="direction">The direction to convert to a vector</param>
    /// <returns>A vector in world-space by the given direction</returns>
    public static Vector2Int ToVector(this Direction direction) {
        return direction switch {
            Direction.LEFT => Vector2Int.left,
            Direction.TOP => Vector2Int.up,
            Direction.RIGHT => Vector2Int.right,
            Direction.BOTTOM => Vector2Int.down,
            _ => Vector2Int.zero
        };
    }

    /// <summary>
    /// Gets the opposite direction of the given direction
    /// </summary>
    /// <param name="direction">The direction to get the opposite direction of</param>
    /// <returns>The opposite direction of the given direction</returns>
    public static Direction GetOpposite(this Direction direction) {
        return direction switch {
            Direction.LEFT => Direction.RIGHT,
            Direction.TOP => Direction.BOTTOM,
            Direction.RIGHT => Direction.LEFT,
            Direction.BOTTOM => Direction.TOP,
            _ => throw new ArgumentException("An illegal direction value for opposite direction was given")
        };
    }
}