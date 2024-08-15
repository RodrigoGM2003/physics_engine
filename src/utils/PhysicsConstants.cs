using System;
using System.Numerics;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;

namespace PhysicsEngine
{
    public static class PhysicsConstants
    {
        public const float PixelsPerMeter = 10.0f;
        public const float LinearDamping = 0.1f;

        public const float Gravity = 9.81f;

        public static Vector2f GravityVector
        {
            get{
                return new Vector2f(0.0f, Gravity);
            }
        }

        public static readonly float DefaultElasticity = 0.5f;
        public static readonly float DefaultFriction = 0.5f;
    }
}