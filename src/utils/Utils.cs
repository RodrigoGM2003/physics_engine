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

        /**
        * Changes the origin of the vertices to center arround (0, 0)
        * @param vertices The vertices of the polygon
        * @return The fixed vertices
        */
        public static Vector2f[] FixVertices(Vector2f[] vertices)
        {
            Vector2f[] fixedVertices = new Vector2f[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                fixedVertices[i] = new Vector2f(vertices[i].X - vertices[0].X, vertices[i].Y - vertices[0].Y);
            }
            return fixedVertices;
        }

        public static int ReadInt(in string line, in string filter)
        {
            // Extract the float value after filter
            string value = line.Substring(filter.Length).Trim();
            return int.Parse(value, ComputingConstants.CultureInfo);
        }
        public static float ReadFloat(in string line, in string filter)
        {
            // Extract the float value after filter
            string value = line.Substring(filter.Length).Trim();
            return float.Parse(value, ComputingConstants.CultureInfo);
        }
        public static Vector2f ReadVector2f(in string line, in string filter)
        {
            // Extract the value after "position:"
            string value = line.Substring(filter.Length).Trim();
            string[] parts = value.Split(',');
            if(parts.Length == 2 && float.TryParse(parts[0], ComputingConstants.CultureInfo, out float x) && 
                                    float.TryParse(parts[1], ComputingConstants.CultureInfo, out float y))
                return new Vector2f(x, y);
            else{
                Console.WriteLine("Invalid position value in body ");
                return new Vector2f(0, 0);
            }
        }
    }
}