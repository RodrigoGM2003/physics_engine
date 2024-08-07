using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;

namespace PhysicsEngine
{
    /**
     * Class for all non-physic bodies in the simulation
     */
    public class PolygonStaticBody : StaticBody
    {
        public Vector2f[] Vertices{ get; protected set; }

        /**
        * Constructor for the StaticBody class
        * @param collider The collider for the body
        * @param drawer The drawer for the body
        */
        public PolygonStaticBody(in Vector2f[] vertices, in RenderWindow window, float? rotation = 0, 
                                Color? color = null, float? elasticity = null, float? friction = null, bool solid = true)
        :base(
            collider: new PolygonCollider(
                position: new Vector2f(0, 0),
                vertices: vertices,
                elasticity: elasticity,
                friction: friction,
                rotation: rotation
            ),
            drawer: new PolygonDrawer(
                _window: window, 
                vertices: Utils.ScaleVertices(vertices, PhysicsConstants.PixelsPerMeter), 
                color: color,
                solid: solid
            ),
            rotation: rotation
        )
        {
            Vertices = vertices;
        }
    }
}