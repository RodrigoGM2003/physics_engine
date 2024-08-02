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
        * @param mass The mass of the object in kg
        * @param collider The collider for the object
        * @param drawer The drawer for the object
        */
        public CircleRigidBody(float radius, in RenderWindow window, Color? color = null, float? elasticity = null, float? friction = null,
                                float? mass = null, Vector2f? velocity = null, Vector2f? acceleration = null)
        : base(new CircleCollider(new Vector2f(0, 0), radius, elasticity, friction), new CircleDrawer(window, radius, color), 
           mass: mass, velocity: velocity, acceleration: acceleration)
        {
            Radius = radius;
        }
    }
}