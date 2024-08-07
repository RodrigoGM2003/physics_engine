using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;

namespace PhysicsEngine
{
    public class PolygonDrawer : Drawer
    {
        public Vector2f[] Vertices { get; protected set; } // The vertices of the polygon
        /**
         * Constructor for the PolygonDrawer class
         * @param window The window to draw to
         * @param radius The radius of the circle
         * @param color The color of the shape
         */
        public PolygonDrawer(in RenderWindow _window,  in Vector2f[] vertices, Color? color = null, bool solid = true)
        : base(_window)
        {
            Vertices = vertices;

            ConvexShape polygon = new ConvexShape((uint)Vertices.Length);
            for (uint i = 0; i < Vertices.Length; i++)
                polygon.SetPoint(i, Vertices[i]);

            shape = polygon;

            // Calculate the centroid and set it as the origin
            Vector2f centroid = CalculateCentroid(Vertices);
            shape.Origin = centroid;
            shape.FillColor = color ?? Color.White;

            SetSolid(solid);
        }

                /**
         * Calculate the centroid of a polygon
         * @param vertices The vertices of the polygon
         * @return The centroid of the polygon
         */
        private Vector2f CalculateCentroid(in Vector2f[] vertices)
        {
            float centroidX = 0, centroidY = 0;
            float signedArea = 0;

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2f current = vertices[i];
                Vector2f next = vertices[(i + 1) % vertices.Length];

                float crossProduct = (current.X * next.Y - current.Y * next.X);
                signedArea += crossProduct;

                centroidX += (current.X + next.X) * crossProduct;
                centroidY += (current.Y + next.Y) * crossProduct;
            }

            signedArea *= 0.5f;
            centroidX /= (6 * signedArea);
            centroidY /= (6 * signedArea);

            return new Vector2f(centroidX, centroidY);
        }
    }
}