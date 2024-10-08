using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;

namespace PhysicsEngine
{
    /**
    * Class for resolving collisions between rigid bodies
    */
    public class CCD : CollisionResolver
    {
        public override bool Discrete => false; // Continuous collision detection is enabled

        public override void ResolveCollision(Body bodyA, Body bodyB, in Vector2f normal, in float depth)
        {
            throw new NotImplementedException();
        }

        public override bool HandleOverlap(Body bodyA, Body bodyB, in Vector2f normal, in float depth)
        {
            throw new NotImplementedException();
        }
    }
}