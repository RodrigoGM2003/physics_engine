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
    public class DCD : CollisionResolver
    {
        public override bool Discrete => true; // Discrete collision detection is enabled

        /**
        * Method to handle an overlap between two rigid bodies
        * @param bodyA The first rigid body
        * @param bodyB The second rigid body
        * @param normal The normal of the collision
        * @param depth The depth of the collision
        * @return Whether the collision must be resolved
        * Implementation:
        * - If one of the bodies is static, move the other body out of the collision
        * - If both bodies are dynamic, move them both out of the collision
        */
        public override bool HandleOverlap(RigidBody bodyA, RigidBody bodyB, in Vector2f normal, in float depth)
        {
            Vector2f aToB = bodyB.Position - bodyA.Position;
            Vector2f bToA = bodyA.Position - bodyB.Position;

            float checkA = aToB.Dot(bodyA.Velocity);
            float checkB = bToA.Dot(bodyB.Velocity);

            if(bodyA.IsStatic)
            {
                if(bToA.Dot(normal) < 0)
                    bodyB.Position += normal * depth * 1.1f;
                else
                    bodyB.Position -= normal * depth * 1.1f;

            }
            else if(bodyB.IsStatic)
            {
                if(aToB.Dot(normal) < 0)
                    bodyA.Position += normal * depth * 1.1f;
                else
                    bodyA.Position -= normal * depth * 1.1f;
            }
            else
            {
                if(bToA.Dot(normal) < 0)
                {
                    bodyA.Position -= normal * depth / 2 * 1.1f;
                    bodyB.Position += normal * depth / 2 * 1.1f;
                }
                else
                {
                    bodyA.Position += normal * depth / 2 * 1.1f;
                    bodyB.Position -= normal * depth / 2 * 1.1f;
                }
            }

            return (checkA < 0 && checkB < 0);
        }

        /**
        * Method to resolve a collision between two rigid bodies
        * @param bodyA The first rigid body
        * @param bodyB The second rigid body
        * @param normal The normal of the collision
        * @param depth The depth of the collision
        * Implementation:
        * - If one of the bodies is static, apply an impulse to the other body
        * - If both bodies are dynamic, apply impulses to both bodies
        */
        public override void ResolveCollision(RigidBody bodyA, RigidBody bodyB, in Vector2f normal, in float depth)
        {
            Vector2f relativeVelocity = bodyA.Velocity - bodyB.Velocity;

            float restitution = Math.Min(bodyA.Collider.Elasticity, bodyB.Collider.Elasticity);

            float j = -(1f + restitution) * relativeVelocity.Dot(normal);

            //30fps
            j /= bodyA.InverseMass + bodyB.InverseMass;
            if (!bodyA.IsStatic)
                bodyA.ApplyImpulse(j * normal);
            
            if (!bodyB.IsStatic)
                bodyB.ApplyImpulse(-j * normal);


            //30fps
            //j /= bodyA.InverseMass + bodyB.InverseMass;
            // if(bodyA.IsStatic)
            //     bodyB.ApplyImpulse(-j * normal);
            // else if(bodyB.IsStatic)
            //     bodyA.ApplyImpulse(j * normal);
            // else
            // {
            //     bodyA.ApplyImpulse(j * normal);
            //     bodyB.ApplyImpulse(-j * normal);
            // }

            //30fps
            // if(bodyA.IsStatic)
            // {
            //     j /= 1f / bodyB.Mass;
            //     bodyB.ApplyImpulse(-j * normal);
            // }
            // else if(bodyB.IsStatic)
            // {
            //     j /= 1f / bodyA.Mass;
            //     bodyA.ApplyImpulse(j * normal);
            // }
            // else
            // {
            //     j /= (1f / bodyA.Mass) + (1f / bodyB.Mass);

            //     bodyA.ApplyImpulse(j * normal);
            //     bodyB.ApplyImpulse(-j * normal);
            // }
        }
    }
}