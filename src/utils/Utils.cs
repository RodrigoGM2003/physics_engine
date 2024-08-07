using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;

namespace PhysicsEngine
{
    /**
     * Class for all physic bodies in the simulation
     */
    public class Utils
    {
        /**
        * Scale the vertices of a polygon
        * @param vertices The vertices of the polygon
        * @param scale The scale to apply to the vertices
        * @return The scaled vertices
        */
        public static Vector2f[] ScaleVertices(Vector2f[] vertices, float scale)
        {
            Vector2f[] scaledVertices = new Vector2f[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                scaledVertices[i] = vertices[i] * scale;
            }
            return scaledVertices;
        }
    }
}