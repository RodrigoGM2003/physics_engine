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
    public class PolygonRigidBody : RigidBody
    {
        public Vector2f[] Vertices{ get; protected set; }

        /**
        * Constructor for the PolygonRigidBody class
        * @param vertices The vertices of the polygon in meters
        * @param window The window to draw the polygon in
        * @param rotation The rotation of the polygon in rads
        * @param color The color of the polygon
        * @param elasticity The elasticity of the polygon
        * @param friction The friction of the polygon
        * @param mass The mass of the polygon in kg
        * @param velocity The velocity of the polygon in m/s
        * @param acceleration The acceleration of the polygon in m/s^2
        * @param angularVelocity The angular velocity of the polygon in rads/s
        */
        public PolygonRigidBody(in Vector2f[] vertices, in RenderWindow window, float? rotation = 0, 
                                Color? color = null, float? elasticity = null, float? friction = null,
                                float? mass = null, Vector2f? velocity = null, Vector2f? acceleration = null, 
                                float? angularVelocity = null, bool solid = true)
        : base(
            new PolygonCollider(
                position: new Vector2f(0, 0),
                vertices: Utils.FixVertices(vertices), 
                elasticity: elasticity, 
                friction: friction, 
                rotation: rotation
            ), 
            new PolygonDrawer(
                _window: window, 
                vertices: Utils.ScaleVertices(vertices, PhysicsConstants.PixelsPerMeter), 
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
            Vertices = vertices;
        }
    }
}