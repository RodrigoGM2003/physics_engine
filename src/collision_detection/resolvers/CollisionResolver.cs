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
    public abstract class CollisionResolver
    {
        public abstract bool Discrete { get; } // Whether continuous collision detection is enabled

        /**
        * Method to resolve a collision between two circle rigid bodies
        * @param circleA The first circle rigid body
        * @param circleB The second circle rigid body
        */
        public abstract void ResolveCollision(CircleRigidBody circleA, CircleRigidBody circleB);
    }
}