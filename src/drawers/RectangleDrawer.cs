using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;

namespace PhysicsEngine
{
    public class RectangleDrawer : Drawer
    {
        public Vector2f Size { get; protected set; } // The radius of the circle

        /**
         * Constructor for the RectangleDrawer class
         * @param window The window to draw to
         * @param radius The radius of the circle
         * @param color The color of the shape
         */
        public RectangleDrawer(in RenderWindow _window,  Vector2f size, Color? color = null)
        : base(_window)
        {
            Size = size;

            shape = new RectangleShape(Size);
            shape.FillColor = color ?? Color.White;

            shape.Origin = new Vector2f(Size.X / 2, Size.Y / 2); // Set origin to the center of the circle
        }
    }
}