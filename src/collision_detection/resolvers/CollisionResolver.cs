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
        * Method to resolve a collision between two rigid bodies
        * @param bodyA The first rigid body
        * @param bodyB The second rigid body
        */

        public void ResolveCollision(Body bodyA, Body bodyB)
        {
            if (bodyA.Collider is CircleCollider && bodyB.Collider is CircleCollider)
                ResolveCircleCollision(bodyA, bodyB);
            
            else if (bodyA.Collider is CircleCollider && bodyB.Collider is PolygonCollider)
                ResolveMixedCollision(bodyA, bodyB);
            
            else if (bodyB.Collider is PolygonCollider && bodyA.Collider is CircleCollider)
                ResolveMixedCollision(bodyB, bodyA);
            
            else if (bodyA.Collider is PolygonCollider && bodyB.Collider is PolygonCollider)
                ResolvePolygonCollision(bodyA, bodyB);
        }


        /**
        * Method to resolve a collision between two circle rigid bodies
        * @param circleA The first circle rigid body
        * @param circleB The second circle rigid body
        */
        protected abstract void ResolveCircleCollision(Body circleA, Body circleB);
        protected abstract void ResolveMixedCollision(Body circle, Body polygon);
        protected abstract void ResolvePolygonCollision(Body polygonA, Body polygonB);
        
    }
}