using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;

namespace PhysicsEngine
{
    /**
     * Class for all non-physic bodies in the simulation
     */
    public class StaticBody : Body
    {
        public Collider SBCollider { get; protected set; } // Collider for the object
        public Drawer? SBDrawer { get; protected set; } // Drawer for the object
        public Vector2f Acceleration { get; set; } // Acceleration in m/s^2
        public Vector2f Velocity { get; set; } // Velocity in m/s

        /**
        * Constructor for the StaticBody class
        * @param collider The collider for the body
        * @param drawer The drawer for the body
        */
        public StaticBody(in Collider collider, in Drawer? drawer = null, Vector2f? velocity = null, Vector2f? acceleration = null)
        {
            Position = new Vector2f(0, 0);
            SBCollider = collider;
            SBDrawer = drawer;
            Velocity = velocity ?? new Vector2f(0, 0);
            Acceleration = acceleration ?? new Vector2f(0, 0);
        }

        /** 
        * Start method for the StaticBody class
        * @param position The position of the object in the scene at start
        */
        public override void Start(Vector2f position)
        {
            Position = position;
        }


        /**
        * Update the state of the StaticBody
        * @param dt The change in time since the last frame
        */
        public override void Update(in float deltaTime)
        {
            Velocity += Acceleration * deltaTime;
            Position += Velocity * deltaTime;
            Acceleration = new Vector2f(0, 0);

            SBCollider.UpdatePosition(Position);
        }

        /**
        * Draw the StaticBody to the screen
        * @param window The window to draw the object to
        *
        * Note: The position of the object is in meters, so we need to convert it to pixels
        */
        public override void Draw()
        {
            if (SBDrawer != null){
                // Convert the floating-point position to pixel position            
                SBDrawer.Draw(Position);
            }
        }
    }
}