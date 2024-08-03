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
    public class CircleRigidBody : RigidBody
    {
        public float Radius { get; protected set; }

        /**
        * Constructor for the CircleRigidBody class
        * @param radius The radius of the circle in meters
        * @param window The window to draw the circle in
        * @param color The color of the circle
        * @param elasticity The elasticity of the circle
        * @param friction The friction of the circle
        * @param mass The mass of the circle
        * @param velocity The velocity of the circle
        * @param acceleration The acceleration of the circle
        */
        public CircleRigidBody(float radius, in RenderWindow window, Color? color = null, float? elasticity = null, float? friction = null,
                                float? mass = null, Vector2f? velocity = null, Vector2f? acceleration = null)
        : base(new CircleCollider(new Vector2f(0, 0), radius, elasticity, friction), 
           new CircleDrawer(window, radius *  PhysicsConstants.PixelsPerMeter, color), 
           mass: mass, velocity: velocity, acceleration: acceleration)
        {
            Radius = radius * PhysicsConstants.PixelsPerMeter;
        }
    }
}