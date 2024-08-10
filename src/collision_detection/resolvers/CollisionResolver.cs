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
            if(bodyA.IsStatic && bodyB.IsStatic){
                Console.WriteLine("Both bodies are static");
                return;
            }

            Vector2f relativeVelocity = bodyA.Velocity - bodyB.Velocity;
            
            float velocityAlongNormal = relativeVelocity.Dot(normal);

            Console.WriteLine("Velocity of A: " + bodyA.Velocity);
            Console.WriteLine("Velocity of B: " + bodyB.Velocity);
            Console.WriteLine("Relative velocity: " + relativeVelocity);
            Console.WriteLine("Velocity along normal: " + velocityAlongNormal);

            // if (velocityAlongNormal > 0)
            // {
            //     // Objects are moving apart, no need to resolve the collision
            //     Console.WriteLine("Objects are moving apart, collision not resolved");
            //     return;
            // }

            float restitution = Math.Min(bodyA.Collider.Elasticity, bodyB.Collider.Elasticity);

            float j = -(1f + restitution) * relativeVelocity.Dot(normal);;

            if(bodyA.IsStatic)
            {
                j /= 1f / bodyB.Mass;
                bodyB.ApplyImpulse(-j * normal);

                bodyB.UpdatePosition(bodyB.Position + normal * depth);
            }
            else if(bodyB.IsStatic)
            {
                j /= 1f / bodyA.Mass;
                bodyA.ApplyImpulse(j * normal);

                // bodyA.UpdatePosition(bodyA.Position - normal * depth);
            }
            else
            {
                j /= (1f / bodyA.Mass) + (1f / bodyB.Mass);
                bodyA.ApplyImpulse(j * normal);
                bodyB.ApplyImpulse(-j * normal);

                bodyA.UpdatePosition(bodyA.Position - normal * depth / 2);
                bodyB.UpdatePosition(bodyB.Position + normal * depth / 2);
            }

            // if(!bodyA.IsStatic)
            //     bodyA.ApplyImpulse(j * normal);

            // if(!bodyB.IsStatic)
            //     bodyB.ApplyImpulse(-j * normal);
        }


        /**
        * Method to resolve a collision between two circle rigid bodies
        * @param circleA The first circle rigid body
        * @param circleB The second circle rigid body
        */
        protected abstract void ResolveCircleCollision(RigidBody bodyA, RigidBody bodyB, in Vector2f normal, in float depth);
        protected abstract void ResolveMixedCollision(RigidBody bodyA, RigidBody bodyB, in Vector2f normal, in float depth);
        protected abstract void ResolvePolygonCollision(RigidBody bodyA, RigidBody bodyB, in Vector2f normal, in float depth);
        
    }
}