using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;

namespace PhysicsEngine
{
    public abstract class Drawer
    {
        public RenderWindow window = null!; // The window to draw to
        public Shape shape {get; protected set;} = null!; // The shape of the object

        /**
        * Constructor for the Drawer class
        * @param window The window to draw to
        */
        public Drawer(in RenderWindow _window)
        {
            window = _window;
        }

        /**
        * Draw the object to the screen
        * @param position The position of the object in the scene
        * @param rotation The rotation of the object in the scene
        */
        public void Draw(Vector2f position, float rotation)
        { 
            //Change the position from meters to pixels
            Vector2f drawPosition = new Vector2f(position.X * PhysicsConstants.PixelsPerMeter, position.Y * PhysicsConstants.PixelsPerMeter);
            shape.Position = drawPosition;

            //Change the rotation from radians to degrees
            float rotationInDegrees = (float)(rotation * 180.0 / Math.PI);
            shape.Rotation = rotationInDegrees;

            window.Draw(shape);
        }


        /**
         * Set the color of the circle
         * @param color The color of the circle
         */
        public void SetColor(Color color)
        {
            shape.FillColor = color;
        }

        /**
         * Get the color of the circle
         * @return The color of the circle
         */
        public Color GetColor()
        {
            return shape.FillColor;
        }
    }
}