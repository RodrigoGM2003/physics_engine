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
    public class Verlet : CollisionResolver
    {
        public override bool Discrete => true; // Discrete collision detection is enabled

        /**
        * Method to handle an overlap between two rigid bodies
        * @param bodyA The first rigid body
        * @param bodyB The second rigid body
        * @param normal The normal of the collision
        * @param depth The depth of the collision
        * @return Whether the overlap was handled
        */
        public override bool HandleOverlap(Body bodyA, Body bodyB, in Vector2f normal, in float depth){

            Vector2f aToB = bodyB.Position - bodyA.Position;
            Vector2f bToA = bodyA.Position - bodyB.Position;

            float checkA = aToB.Dot(bodyA.Velocity);
            float checkB = bToA.Dot(bodyB.Velocity);

            if(bodyA.IsStatic)
            {
                if(bToA.Dot(normal) < 0)
                    bodyB.Position += normal * depth ;//* 1.1f;
                else
                    bodyB.Position -= normal * depth ;//* 1.1f;

            }
            else if(bodyB.IsStatic)
            {
                if(aToB.Dot(normal) < 0)
                    bodyA.Position += normal * depth ;//* 1.1f;
                else
                    bodyA.Position -= normal * depth ;//* 1.1f;
            }
            else
            {
                if(bToA.Dot(normal) < 0)
                {
                    bodyA.Position -= normal * depth / 2 ;//* 1.1f;
                    bodyB.Position += normal * depth / 2 ;//* 1.1f;
                }
                else
                {
                    bodyA.Position += normal * depth / 2 ;//* 1.1f;
                    bodyB.Position -= normal * depth / 2 ;//* 1.1f;
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
        */
        public override void ResolveCollision(Body bodyA, Body bodyB, in Vector2f normal, in float depth){
            //No need to resolve collisions in Verlet as Velocity Verlet integration will handle it
        }
    }
}