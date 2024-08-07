using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;

namespace PhysicsEngine
{
    public class CircleDrawer : Drawer
    {
        public float Radius { get; protected set; } // The radius of the circle

        /**
         * Constructor for the CircleDrawer class
         * @param window The window to draw to
         * @param radius The radius of the circle
         * @param color The color of the shape
         */
        public CircleDrawer(in RenderWindow _window,  float radius, Color? color = null, bool solid = true)
        : base(_window)
        {
            Radius = radius;

            shape = new CircleShape(radius, ComputingConstants.CirclePointCount);

            shape.Origin = new Vector2f(Radius, Radius); // Set origin to the center of the circle
            shape.FillColor = color ?? Color.White;

            SetSolid(solid);
        }
    }
}