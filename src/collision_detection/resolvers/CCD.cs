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
        /**
        * Method to resolve a collision between two circle rigid bodies
        * @param circleA The first circle rigid body
        * @param circleB The second circle rigid body
        * @param toi The time of impact
        * @param deltaTime The change in time since the last frame
        */
        protected override void ResolveCircleCollision(Body circleA, Body circleB)//, float toi, float deltaTime)
        {

        }
        protected override void ResolveMixedCollision(Body circleA, Body circleB)//, float toi, float deltaTime)
        {

        }
        protected override void ResolvePolygonCollision(Body circleA, Body circleB)//, float toi, float deltaTime)
        {

        }
    }
}