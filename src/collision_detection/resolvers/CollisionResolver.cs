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
        * Method to resolve a collision between two rigid bodies
        * @param bodyA The first rigid body
        * @param bodyB The second rigid body
        */
        public void ResolveCollision(RigidBody bodyA, RigidBody bodyB, in Vector2f normal, in float depth)
        {
            if(bodyA.IsStatic && bodyB.IsStatic)
                return;
            
            Vector2f relativeVelocity = bodyA.Velocity - bodyB.Velocity;

            float restitution = Math.Min(bodyA.Collider.Elasticity, bodyB.Collider.Elasticity);

            float j = -(1f + restitution) * relativeVelocity.Dot(normal);;

            // Console.WriteLine("------------------------------------------");
            // Console.WriteLine("Normal: " + normal);
            // Console.WriteLine("Depth: " + depth);
            // Console.WriteLine("Body A Position: " + bodyA.Position);
            // Console.WriteLine("Body B Position: " + bodyB.Position);
            // Console.WriteLine("Body A velocity: " + bodyA.Velocity);
            // Console.WriteLine("Body B velocity: " + bodyB.Velocity);
            // Console.WriteLine("Relative velocity: " + relativeVelocity);
            // Console.WriteLine("Velocity along normal: " + velocityAlongNormal);

            if(bodyA.IsStatic)
            {
                j /= 1f / bodyB.Mass;
                bodyB.ApplyImpulse(-j * normal);

                // Console.WriteLine("Body B Impulse: " + -j * normal);
            }
            else if(bodyB.IsStatic)
            {
                j /= 1f / bodyA.Mass;
                bodyA.ApplyImpulse(j * normal);

                // Console.WriteLine("Body A Impulse: " + j * normal);
            }
            else
            {
                j /= (1f / bodyA.Mass) + (1f / bodyB.Mass);

                bodyA.ApplyImpulse(j * normal);
                bodyB.ApplyImpulse(-j * normal);

                // Console.WriteLine("Body A Impulse: " + j * normal);
                // Console.WriteLine("Body B Impulse: " + -j * normal);
            }

            // Console.WriteLine("------------------------------------------");

        }        
    }
}