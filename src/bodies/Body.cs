using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;
using System.Numerics;

namespace PhysicsEngine
{
    /**
     * Class for all physic bodies in the simulation
     */
    public abstract class Body //: Body
    {
        protected Vector2f _position; // Position in m
        protected bool _isStatic; // Rotation in rads

        public Vector2f Velocity { get; set; } // Velocity in m/s

        public Collider Collider { get; protected set; } // Collider for the object
        public Drawer Drawer { get; protected set; }

        public abstract bool IsStatic {
            get;
            set;
        } // Property for the staticness of the object

        public abstract Vector2f Position // Property for the position of the object
        {
            get;
            set;
        }


        /** 
        * Start method for the RigidBody class
        * @param position The position of the object in the scene at start
        */
        public abstract void Start(Vector2f position);
        /**
        * Update the state of the RigidBody
        * @param dt The change in time since the last frame
        */
        public abstract void Update(in float deltaTime);

        /**
        * Draw the RigidBody to the screen
        * @param window The window to draw the object to
        *
        * Note: The position of the object is in meters, so we need to convert it to pixels
        */
        public abstract void Draw();
    }
}