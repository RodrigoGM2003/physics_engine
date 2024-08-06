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
    public class RigidBody : Body
    {
        public Collider RBCollider { get; protected set; }
        public Drawer RBDrawer { get; protected set; }
        public Vector2f Velocity { get; set; } // Velocity in m/s
        public Vector2f Acceleration { get; set; } // Acceleration in m/s^2
        public float AngularVelocity { get; set; } // Angular velocity in rads/s
        public float AngularAcceleration { get; set; } // Angular acceleration in rads/s^2
        public float Mass { get; set; } // Mass in kg


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
                        Vector2f? acceleration = null, float? mass = null, float? angularVelocity = null)
        {
            Position = new Vector2f(0, 0);
            Rotation = rotation ?? 0;
            RBCollider = collider;
            RBDrawer = drawer;
            Velocity = velocity ?? new Vector2f(0, 0);
            Acceleration = acceleration ?? new Vector2f(0, 0);
            AngularVelocity = angularVelocity ?? 0;
            Mass = mass ?? 1;
        }

        /** 
        * Start method for the RigidBody class
        * @param position The position of the object in the scene at start
        */
        public override void Start(Vector2f position)
        {
            Position = position;
            RBCollider.UpdatePosition(Position, Rotation);
            RBCollider.UpdateSweptAABB();
        }

        /**
        * Update the state of the RigidBody
        * @param dt The change in time since the last frame
        */
        public override void Update(in float deltaTime)
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

            // Apply gravity if the object has mass
            if (Mass > 0)
                ApplyAcceleration(PhysicsConstants.GravityVector);
            
            //Update the colliders position
            RBCollider.UpdatePosition(Position, Rotation);
            RBCollider.UpdateSweptAABB();
        }

        /**
        * Draw the RigidBody to the screen
        * @param window The window to draw the object to
        *
        * Note: The position of the object is in meters, so we need to convert it to pixels
        */
        public override void Draw()
        {   
            RBDrawer.Draw(Position, Rotation);

            RectangleShape shape = new RectangleShape(RBCollider.SweptAABB.Size * PhysicsConstants.PixelsPerMeter);
            shape.Position = new Vector2f(RBCollider.SweptAABB.Left * PhysicsConstants.PixelsPerMeter, RBCollider.SweptAABB.Top * PhysicsConstants.PixelsPerMeter);
            shape.FillColor = Color.Transparent;
            shape.OutlineColor = Color.Red;
            shape.OutlineThickness = 1;

            RenderWindowManager.Window.Draw(shape);

            CircleShape circle = new CircleShape(RBCollider.BoundingBox.Width / 2 * PhysicsConstants.PixelsPerMeter, 30);
            circle.Position = new Vector2f(RBCollider.LastPosition.X * PhysicsConstants.PixelsPerMeter, RBCollider.LastPosition.Y * PhysicsConstants.PixelsPerMeter);
            circle.Origin = new Vector2f(RBCollider.BoundingBox.Width / 2 * PhysicsConstants.PixelsPerMeter, RBCollider.BoundingBox.Height / 2 * PhysicsConstants.PixelsPerMeter);
            circle.FillColor = Color.Transparent;
            circle.OutlineColor = Color.Red;
            circle.OutlineThickness = 1;

            RenderWindowManager.Window.Draw(circle);

            // circle = new CircleShape(RBCollider.BoundingBox.Width / 2 * PhysicsConstants.PixelsPerMeter, 30);
            // circle.Position = new Vector2f(Position.X * PhysicsConstants.PixelsPerMeter, Position.Y * PhysicsConstants.PixelsPerMeter);
            // circle.Origin = new Vector2f(RBCollider.BoundingBox.Width / 2 * PhysicsConstants.PixelsPerMeter, RBCollider.BoundingBox.Height / 2 * PhysicsConstants.PixelsPerMeter);
            // circle.FillColor = Color.Blue;
            // circle.OutlineColor = Color.Blue;
            // circle.OutlineThickness = 1;   
            // RenderWindowManager.Window.Draw(circle);
        }

        public void UpdatePosition(Vector2f position)
        {
            Position = position;
            RBCollider.UpdatePosition(position, Rotation);
            RBCollider.UpdateSweptAABB();
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