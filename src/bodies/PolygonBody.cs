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
        * @param mass The mass of the object in kg
        * @param collider The collider for the object
        * @param drawer The drawer for the object
        */
        public PolygonRigidBody(in Vector2f[] vertices, in RenderWindow window, Color? color = null, float? elasticity = null, float? friction = null,
                                float? mass = null, Vector2f? velocity = null, Vector2f? acceleration = null)
        : base(new PolygonCollider(new Vector2f(0, 0), vertices, elasticity, friction), 
           new PolygonDrawer(window, ScaleVertices(vertices, PhysicsConstants.PixelsPerMeter), color), 
           mass: mass, velocity: velocity, acceleration: acceleration)
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