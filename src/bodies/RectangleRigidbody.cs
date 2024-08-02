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
        * Constructor for the RectangleRigidBody class
        * @param mass The mass of the object in kg
        * @param collider The collider for the object
        * @param drawer The drawer for the object
        */
        public RectangleRigidBody(Vector2f size, in RenderWindow window, Color? color = null, float? elasticity = null, float? friction = null,
                                float? mass = null, Vector2f? velocity = null, Vector2f? acceleration = null)
        : base(new RectangleCollider(new Vector2f(0, 0), size, elasticity, friction), new RectangleDrawer(window, size, color), 
           mass: mass, velocity: velocity, acceleration: acceleration)
        {
            Size = size;
        }
    }
}