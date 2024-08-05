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
        public PolygonRigidBody(in Vector2f[] vertices, in RenderWindow window, float? rotation = 0, Color? color = null, float? elasticity = null, float? friction = null,
                                float? mass = null, Vector2f? velocity = null, Vector2f? acceleration = null, float? angularVelocity = null)
        : base(
            new PolygonCollider(
                position: new Vector2f(0, 0), 
                vertices: vertices, 
                elasticity: elasticity, 
                friction: friction, 
                rotation: rotation
            ), 
            new PolygonDrawer(
                _window: window, 
                vertices: ScaleVertices(vertices, PhysicsConstants.PixelsPerMeter), 
                color: color
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

        private static Vector2f[] ScaleVertices(Vector2f[] vertices, float scale)
        {
            Vector2f[] scaledVertices = new Vector2f[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                scaledVertices[i] = vertices[i] * scale;
            }
            return scaledVertices;
        }
    }
}