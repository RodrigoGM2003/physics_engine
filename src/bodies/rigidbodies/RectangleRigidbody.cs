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
    public class RectangleRigidBody : RigidBody
    {
        public Vector2f Size { get; protected set; }

        /**
        * Constructor for the CircleRigidBody class
        * @param size The size of the rectangle in meters
        * @param window The window to draw the circle in
        * @param rotation The rotation of the circle in rads
        * @param color The color of the circle
        * @param elasticity The elasticity of the circle
        * @param friction The friction of the circle
        * @param mass The mass of the circle in kg
        * @param velocity The velocity of the circle in m/s
        * @param acceleration The acceleration of the circle in m/s^2
        * @param angularVelocity The angular velocity of the circle in rads/s
        */
        public RectangleRigidBody(Vector2f size, in RenderWindow window, float? rotation = 0, 
                                Color? color = null, float? elasticity = null, float? friction = null,
                                float? mass = null, Vector2f? velocity = null, Vector2f? acceleration = null, 
                                float? angularVelocity = null, bool solid = true)
        : base(
            new PolygonCollider(
                position: new Vector2f(0, 0), 
                vertices: new Vector2f[] {
                    new Vector2f(-size.X / 2, -size.Y / 2),
                    new Vector2f(-size.X / 2, size.Y / 2),
                    new Vector2f(size.X / 2, size.Y / 2),
                    new Vector2f(size.X / 2, -size.Y / 2),
                    
                }, 
                elasticity: elasticity, 
                friction: friction, 
                rotation: rotation
            ), 
            new RectangleDrawer(
                _window: window, 
                size: size * PhysicsConstants.PixelsPerMeter, 
                color: color,
                solid: solid
            ), 
            mass: mass, 
            velocity: velocity, 
            acceleration: acceleration, 
            rotation: rotation,
            angularVelocity: angularVelocity 
        )
        {
            Size = size;
        }
    }
}