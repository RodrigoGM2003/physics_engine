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
    }
}