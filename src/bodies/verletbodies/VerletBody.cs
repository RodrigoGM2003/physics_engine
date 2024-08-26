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
    public class VerletBody : Body
    {
        protected Vector2f _oldPosition; // Position in m
        public Vector2f Acceleration { get; set; } // Acceleration in m/s^2
        public override Vector2f Position // Property for the position of the object
        {
            get => _position;
            set => _position = value;
        }
        public Vector2f OldPosition // Property for the old position of the object
        {
            get => _oldPosition;
            set => _oldPosition = value;
        }
        public override bool IsStatic // Property for the staticness of the object
        {
            get => _isStatic;
            set => _isStatic = value;
        }

        /**
        * Constructor for the RigidBody class
        * @param collider The collider for the object
        * @param drawer The drawer for the object
        * @param rotation The rotation of the object in rads
        * @param velocity The velocity of the object in m/s
        * @param acceleration The acceleration of the object in m/s^2
        * @param mass The mass of the object in kg
        * @param angularVelocity The angular velocity of the object in rads/s
        */
        public VerletBody(in Collider collider, in Drawer drawer, Vector2f? velocity = null, bool isStatic = false)
        {
            Collider = collider;
            Drawer = drawer;

            Position = new Vector2f(0, 0);
            OldPosition = new Vector2f(0, 0);

            Velocity = velocity ?? new Vector2f(0, 0);
            Acceleration = new Vector2f(0, 0);

            IsStatic = isStatic;
        }

        /** 
        * Start method for the RigidBody class
        * @param position The position of the object in the scene at start
        */
        public override void Start(Vector2f position)
        {
            _position = position;
            _oldPosition = position;
        }

        /**
        * Update the state of the RigidBody
        * @param dt The change in time since the last frame
        */
        public override void Update(in float deltaTime)
        {
            Velocity = _position - _oldPosition;
            _oldPosition = _position;
            _position = _position + Velocity + (Acceleration * deltaTime * deltaTime);

            Acceleration = new Vector2f(0, 0);

            
            if (!IsStatic)
                // Apply gravity if the object has mass
                ApplyAcceleration(PhysicsConstants.GravityVector);
            // ApplyAcceleration(PhysicsConstants.GravityVector);
            
            //Update the colliders position
            Collider.UpdatePosition(_position, 0);
            Collider.UpdateSweptAABB();
        }

        /**
        * Draw the RigidBody to the screen
        * @param window The window to draw the object to
        *
        * Note: The position of the object is in meters, so we need to convert it to pixels
        */
        public override void Draw()
        {   
            Drawer.Draw(Position, 0);

            // RectangleShape shape = new RectangleShape(Collider.SweptAABB.Size * PhysicsConstants.PixelsPerMeter);
            // shape.Position = new Vector2f(Collider.SweptAABB.Left * PhysicsConstants.PixelsPerMeter, Collider.SweptAABB.Top * PhysicsConstants.PixelsPerMeter);
            // shape.FillColor = Color.Transparent;
            // shape.OutlineColor = Color.Red;
            // shape.OutlineThickness = 1;

            // RenderWindowManager.Window.Draw(shape);

            // CircleShape circle = new CircleShape(Collider.BoundingBox.Width / 2 * PhysicsConstants.PixelsPerMeter, 30);
            // circle.Position = new Vector2f(Collider.LastPosition.X * PhysicsConstants.PixelsPerMeter, Collider.LastPosition.Y * PhysicsConstants.PixelsPerMeter);
            // circle.Origin = new Vector2f(Collider.BoundingBox.Width / 2 * PhysicsConstants.PixelsPerMeter, Collider.BoundingBox.Height / 2 * PhysicsConstants.PixelsPerMeter);
            // circle.FillColor = Color.Transparent;
            // circle.OutlineColor = Color.Red;
            // circle.OutlineThickness = 1;

            // RenderWindowManager.Window.Draw(circle);

            // circle = new CircleShape(Collider.BoundingBox.Width / 2 * PhysicsConstants.PixelsPerMeter, 30);
            // circle.Position = new Vector2f(Position.X * PhysicsConstants.PixelsPerMeter, Position.Y * PhysicsConstants.PixelsPerMeter);
            // circle.Origin = new Vector2f(Collider.BoundingBox.Width / 2 * PhysicsConstants.PixelsPerMeter, Collider.BoundingBox.Height / 2 * PhysicsConstants.PixelsPerMeter);
            // circle.FillColor = Color.Blue;
            // circle.OutlineColor = Color.Blue;
            // circle.OutlineThickness = 1;   
            // RenderWindowManager.Window.Draw(circle);
        }


        /**
        * Apply an acceleration to the RigidBody
        * @param acceleration The acceleration to apply to the object
        */
        public void ApplyAcceleration(Vector2f acceleration)
        {
            Acceleration += acceleration;
        }

    }
}