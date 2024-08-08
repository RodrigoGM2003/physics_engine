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
        public Drawer? SBDrawer { get; protected set; } // Drawer for the object


        /**
        * Constructor for the StaticBody class
        * @param collider The collider for the body
        * @param drawer The drawer for the body
        */
        public StaticBody(in Collider collider, in Drawer? drawer = null, float? rotation = 0)
        :base(collider)
        {
            Position = new Vector2f(0, 0);
            Rotation = rotation ?? 0;
            SBDrawer = drawer;
        }

        /** 
        * Start method for the StaticBody class
        * @param position The position of the object in the scene at start
        */
        public override void Start(Vector2f position)
        {
            Position = position;
            Collider.UpdatePosition(Position, Rotation);
            Collider.UpdateSweptAABB();
        }


        /**
        * Update the state of the StaticBody
        * @param dt The change in time since the last frame
        */
        public override void Update(in float deltaTime)
        {
            //Update the colliders position
            Collider.UpdatePosition(Position, Rotation);
            Collider.UpdateSweptAABB();
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
                SBDrawer.Draw(Position, Rotation);
            }
        }
    }
}