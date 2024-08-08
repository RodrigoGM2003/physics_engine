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
    public class RectangleStaticBody : StaticBody
    {
        public Vector2f Size{ get; protected set; }

        /**
        * Constructor for the StaticBody class
        * @param collider The collider for the body
        * @param drawer The drawer for the body
        */
        public RectangleStaticBody(in Vector2f size, in RenderWindow window, float? rotation = 0, 
                                Color? color = null, float? elasticity = null, float? friction = null, 
                                bool solid = true)
        :base(
            collider: new PolygonCollider(
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
            drawer: new RectangleDrawer(
                _window: window, 
                size: size * PhysicsConstants.PixelsPerMeter,
                color: color,
                solid: solid
            ),
            rotation: rotation
        )
        
        {
            Size = size;
        }
    }
}