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
    public class RigidBody //: Body
    {
        private float _mass; // Mass in kg
        private float _inverseMass; // Inverse mass in kg^-1
        private bool _isStatic; // Whether the object is static or dynamic
        private Vector2f _position; // Position in m
        private float _rotation; // Rotation in grads

        public Collider Collider { get; protected set; } // Collider for the object
        public Drawer RBDrawer { get; protected set; }
        public Vector2f Velocity { get; set; } // Velocity in m/s
        public Vector2f Acceleration { get; set; } // Acceleration in m/s^2
        public float AngularVelocity { get; set; } // Angular velocity in rads/s
        public float AngularAcceleration { get; set; } // Angular acceleration in rads/s^2
        public Vector2f Position // Property for the position of the object
        {
            get => _position;
            set
            {
                _position = value;
                Collider.UpdatePosition(_position, _rotation);
                Collider.UpdateSweptAABB();
            }
        }

        public float Rotation // Property for the rotation of the object
        {
            get => _rotation;
            set
            {
                _rotation = value;
                Collider.UpdatePosition(_position, _rotation);
                Collider.UpdateSweptAABB();
            }
        }
        public float Mass // Property for the mass of the object
        {
            get => _mass;
            set
            {
                _mass = value;
                UpdateInverseMass();
            }
        }
        public float InverseMass => _inverseMass; // Property for the inverse mass of the object
        public bool IsStatic // Property for the staticness of the object
        {
            get => _isStatic;
            set
            {
                _isStatic = value;
                UpdateInverseMass();
            }
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
        public RigidBody(in Collider collider, in Drawer drawer, float? rotation = 0, Vector2f? velocity = null, 
                        Vector2f? acceleration = null, float? mass = null, float? angularVelocity = null, 
                        float? angularAcceleration = null, bool isStatic = false)
        {
            Collider = collider;
            RBDrawer = drawer;

            Position = new Vector2f(0, 0);
            Rotation = rotation ?? 0;

            Mass = mass ?? 1;

            Velocity = velocity ?? new Vector2f(0, 0);
            Acceleration = acceleration ?? new Vector2f(0, 0);
            AngularVelocity = angularVelocity ?? 0;
            AngularAcceleration = angularAcceleration ?? 0;

            IsStatic = isStatic;
        }

        /** 
        * Start method for the RigidBody class
        * @param position The position of the object in the scene at start
        */
        public void Start(Vector2f position)
        {
            Position = position;
        }

        /**
        * Update the state of the RigidBody
        * @param dt The change in time since the last frame
        */
        public void Update(in float deltaTime)
        {
            Velocity += Acceleration * deltaTime;
            Position += Velocity * deltaTime;
            AngularVelocity += AngularAcceleration * deltaTime;
            Rotation = Rotation + AngularVelocity * deltaTime % (MathF.PI * 2);
            AngularAcceleration = 0;
            Acceleration = new Vector2f(0, 0);
            if (!IsStatic)
            {
                // Apply gravity if the object has mass
                ApplyAcceleration(PhysicsConstants.GravityVector);
            }
            //Update the colliders position
            Collider.UpdatePosition(Position, Rotation);
            Collider.UpdateSweptAABB();
        }

        /**
        * Draw the RigidBody to the screen
        * @param window The window to draw the object to
        *
        * Note: The position of the object is in meters, so we need to convert it to pixels
        */
        public void Draw()
        {   
            RBDrawer.Draw(Position, Rotation);

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
        * Apply a force to the RigidBody
        * @param force The force to apply to the object
        */
        public void ApplyForce(Vector2f force)
        {
            Acceleration += force / Mass;
        }

        /**
        * Apply an impulse to the RigidBody
        * @param impulse The impulse to apply to the object
        *
        * Note: Impulse is a "sudden" change in velocity, so we divide by mass to get the acceleration
        */
        public void ApplyImpulse(Vector2f impulse)
        {
            Velocity += impulse / Mass;
        }

        /**
        * Apply an acceleration to the RigidBody
        * @param acceleration The acceleration to apply to the object
        */
        public void ApplyAcceleration(Vector2f acceleration)
        {
            Acceleration += acceleration;
        }

        /**
        * Update the value of inverse mass of the object
        */
        private void UpdateInverseMass() // Method to update the inverse mass of the object
        {
            _inverseMass = _isStatic ? 0 : 1 / _mass;
        }
    }
}