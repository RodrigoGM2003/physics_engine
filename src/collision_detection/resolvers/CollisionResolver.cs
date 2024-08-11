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

            Vector2f aToB = bodyB.Position - bodyA.Position;
            Vector2f bToA = bodyA.Position - bodyB.Position;

            float checkA = aToB.Dot(bodyA.Velocity);
            float checkB = bToA.Dot(bodyB.Velocity);

            //If the objects are moving away from each other, no need to resolve the collision
            if(checkA < 0 && checkB < 0){
                // Console.WriteLine("Objects are moving apart, collision not resolved");
                return;
            }

            
            float velocityAlongNormal = relativeVelocity.Dot(normal);

            // Console.WriteLine("------------------------------------------");
            // Console.WriteLine("Normal: " + normal);
            // Console.WriteLine("Depth: " + depth);
            // Console.WriteLine("Body A Position: " + bodyA.Position);
            // Console.WriteLine("Body B Position: " + bodyB.Position);
            // Console.WriteLine("Body A velocity: " + bodyA.Velocity);
            // Console.WriteLine("Body B velocity: " + bodyB.Velocity);
            // Console.WriteLine("Relative velocity: " + relativeVelocity);
            // Console.WriteLine("Velocity along normal: " + velocityAlongNormal);



            // if (velocityAlongNormal > 0)
            // {
            //     // Objects are moving apart, no need to resolve the collision
            // // //     Console.WriteLine("Objects are moving apart, collision not resolved");
            //     return;
            // }

            float restitution = Math.Min(bodyA.Collider.Elasticity, bodyB.Collider.Elasticity);

            float j = -(1f + restitution) * relativeVelocity.Dot(normal);;

            if(bodyA.IsStatic)
            {
                j /= 1f / bodyB.Mass;
                bodyB.ApplyImpulse(-j * normal);

                if(bToA.Dot(normal) < 0)
                    bodyB.UpdatePosition(bodyB.Position + normal * depth);
                
                else
                    bodyB.UpdatePosition(bodyB.Position - normal * depth);

                // bodyB.UpdatePosition(bodyB.Position - normal * depth);

                // Console.WriteLine("Body B Impulse: " + -j * normal);
                // Console.WriteLine("Body B moved: " + normal * depth);
            }
            else if(bodyB.IsStatic)
            {
                j /= 1f / bodyA.Mass;
                bodyA.ApplyImpulse(j * normal);
                bodyA.UpdatePosition(bodyA.Position - normal * depth);

                if(aToB.Dot(normal) < 0)
                    bodyA.UpdatePosition(bodyA.Position + normal * depth);
                
                else
                    bodyA.UpdatePosition(bodyA.Position - normal * depth);

                // Console.WriteLine("Body A Impulse: " + j * normal);
                // Console.WriteLine("Body A moved: " + normal * depth);
            }
            else
            {
                j /= (1f / bodyA.Mass) + (1f / bodyB.Mass);
                // bodyA.UpdatePosition(bodyA.Position - normal * depth / 2);
                // bodyB.UpdatePosition(bodyB.Position + normal * depth / 2);
                if(bToA.Dot(normal) < 0)
                {
                    bodyB.UpdatePosition(bodyB.Position + normal * depth);
                }

                bodyA.ApplyImpulse(j * normal);
                bodyB.ApplyImpulse(-j * normal);

                // Console.WriteLine("Body A Impulse: " + j * normal);
                // Console.WriteLine("Body B Impulse: " + -j * normal);

            }

            // // Console.WriteLine("------------------------------------------");

        }        
    }
}