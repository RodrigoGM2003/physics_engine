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

        // SAP theorem
        //Ecuation to calculate the normal of an edge
        //IMPORTANT: The order is counter-clockwise as every thing else is
        //normal = y, -x

        //SAP procedure
        //Grab one of the 2 polygons
        //  Calculate the normal one edge
        //  Project all the vertices onto the normal (dot product of the vertex and the normal)
        //  Calculate max and min of both polygons
        //  If they intersect, continue
        //  If they don't, return false (no collision)
        //Repeat for the other polygon
        //If both polygons intersect, return true (collision)




        public override bool Discrete => true; // Discrete collision detection is enabled
        /**
        * Method to resolve a collision between two circle rigid bodies
        * @param circleA The first circle rigid body
        * @param circleB The second circle rigid body
        * @param toi The time of impact
        * @param deltaTime The change in time since the last frame
        */
        protected override void ResolveCircleCollision(RigidBody circleA, RigidBody circleB, in Vector2f normal, in float depth){
            Console.WriteLine("Circle collision");
        }

        /**
        * Method to resolve a collision between a circle and a polygon rigid body
        * @param circle The circle rigid body
        * @param polygon The polygon rigid body
        */
        protected override void ResolveMixedCollision(RigidBody circleA, RigidBody circleB, in Vector2f normal, in float depth)
        {
            Console.WriteLine("Mixed collision");
        }

        /**
        * Method to resolve a collision between a 2 polygons
        * @param polygonA The first polygon rigid body
        * @param polygonB The second polygon rigid body
        */
        protected override void ResolvePolygonCollision(RigidBody circleA, RigidBody circleB, in Vector2f normal, in float depth)
        {
            Console.WriteLine("Polygon collision");
        }

    }
}