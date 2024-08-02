using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;
using System.Numerics;

public class AABB
{
    public Vector2f Min { get; private set; }
    public Vector2f Max { get; private set; }

    public AABB(Vector2f min, Vector2f max)
    {
        Min = min;
        Max = max;
    }

    public bool Intersects(in AABB other)
    {
        return Min.X <= other.Max.X && Max.X >= other.Min.X &&
                Min.Y <= other.Max.Y && Max.Y >= other.Min.Y;
    }
}
