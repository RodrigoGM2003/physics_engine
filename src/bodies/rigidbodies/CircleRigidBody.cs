using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;
using System.Formats.Tar;

namespace PhysicsEngine
{
    /**
     * Class for all physic bodies in the simulation
     */
    public class CircleRigidBody : RigidBody
    {
        public float Radius { get; protected set; }

        /**
        * Constructor for the CircleRigidBody class
        * @param radius The radius of the circle in meters
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
        public CircleRigidBody(float radius, in RenderWindow window, float? rotation = 0, 
                            Color? color = null, float? elasticity = null, float? friction = null, 
                            float? mass = null, Vector2f? velocity = null, Vector2f? acceleration = null, 
                            float? angularVelocity = null, float? angularAcceleration = null, 
                            bool isStatic = false, bool solid = true)
        : base(
            new CircleCollider(
                position: new Vector2f(0, 0), 
                radius: radius, 
                elasticity: elasticity, 
                friction: friction, 
                rotation: rotation
            ), 
            new CircleDrawer(
                _window: window, 
                radius: radius *  PhysicsConstants.PixelsPerMeter, 
                color: color,
                solid: solid
            ), 
            mass: mass, 
            velocity: velocity, 
            acceleration: acceleration, 
            rotation: rotation,
            angularVelocity: angularVelocity,
            angularAcceleration: angularAcceleration,
            isStatic: isStatic
        )
        {
            Radius = radius;
        }
        public CircleRigidBody(in RenderWindow window)
        : base(
            new CircleCollider(
                position: new Vector2f(0, 0), 
                radius: 1, 
                elasticity: PhysicsConstants.DefaultElasticity, 
                friction: PhysicsConstants.DefaultFriction, 
                rotation: 0
            ), 
            new CircleDrawer(
                _window: window, 
                radius: 1 *  PhysicsConstants.PixelsPerMeter, 
                color: Color.White,
                solid: true
            ), 
            mass: 1, 
            velocity: new Vector2f(0, 0),
            acceleration: new Vector2f(0, 0),
            rotation: 0,
            angularVelocity: 0,
            angularAcceleration: 0,
            isStatic: false
        )
        {
            Radius = 1;
        }
    }
}