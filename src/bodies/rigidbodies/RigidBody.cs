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
        public Vector2f Position { get; protected set; } // Position in m
        public float Rotation { get; protected set; } // Rotation in grads
        public Collider Collider { get; set; } // Collider for the object
        public Drawer RBDrawer { get; protected set; }
        public float Mass { get; set; } // Mass in kg
        public Vector2f Velocity { get; set; } // Velocity in m/s
        public Vector2f Acceleration { get; set; } // Acceleration in m/s^2
        public float AngularVelocity { get; set; } // Angular velocity in rads/s
        public float AngularAcceleration { get; set; } // Angular acceleration in rads/s^2
        public bool IsStatic { get; set; } // Is the object static


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
            Position = new Vector2f(0, 0);
            Rotation = rotation ?? 0;
            Collider = collider;
            RBDrawer = drawer;
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
            Collider.UpdatePosition(Position, Rotation);
            Collider.UpdateSweptAABB();
        }

        /**
        * Update the state of the RigidBody
        * @param dt The change in time since the last frame
        */
        public void Update(in float deltaTime)
        {
            if (!IsStatic)
            {
                Velocity += Acceleration * deltaTime;
                // Velocity *= (1 - LinearDamping * deltaTime);
                Position += Velocity * deltaTime;
                Acceleration = new Vector2f(0, 0);

                AngularVelocity += AngularAcceleration * deltaTime;
                // AngularVelocity *= (1 - AngularDamping * deltaTime);
                Rotation += AngularVelocity * deltaTime;
                if (Rotation > MathF.PI * 2)
                    Rotation -= MathF.PI * 2;

                AngularAcceleration = 0;

                // Apply gravity if the object has mass
                // ApplyAcceleration(PhysicsConstants.GravityVector);
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

        public void UpdatePosition(Vector2f position)
        {
            Position = position;
            Collider.UpdatePosition(position, Rotation);
            Collider.UpdateSweptAABB();
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
    }
}