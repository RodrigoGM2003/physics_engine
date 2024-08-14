using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;
using System.Text;

namespace PhysicsEngine
{
    /**
    * Class for resolving collisions between rigid bodies
    */
    public abstract class CollisionResolver
    {
        public abstract bool Discrete { get; } // Whether continuous collision detection is enabled

        /**
        * Method to handle an overlap between two rigid bodies
        * @param bodyA The first rigid body
        * @param bodyB The second rigid body
        * @param normal The normal of the collision
        * @param depth The depth of the collision
        * @return Whether the overlap was handled
        */
        public abstract bool HandleOverlap(RigidBody bodyA, RigidBody bodyB, in Vector2f normal, in float depth);

        /**
        * Method to resolve a collision between two rigid bodies
        * @param bodyA The first rigid body
        * @param bodyB The second rigid body
        * @param normal The normal of the collision
        * @param depth The depth of the collision
        */
        public abstract void ResolveCollision(RigidBody bodyA, RigidBody bodyB, in Vector2f normal, in float depth);  



    }
}