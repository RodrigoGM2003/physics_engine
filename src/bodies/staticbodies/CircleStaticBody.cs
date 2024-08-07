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
    public class CircleStaticBody : StaticBody
    {
        public float Radius{ get; protected set; }

        /**
        * Constructor for the StaticBody class
        * @param radius The radius of the circle in meters
        * @param window The window to draw the circle in
        * @param rotation The rotation of the circle in rads
        * @param color The color of the circle
        * @param elasticity The elasticity of the circle
        * @param friction The friction of the circle
        */
        public CircleStaticBody(in float radius, in RenderWindow window, float? rotation = 0, 
                                Color? color = null, float? elasticity = null, float? friction = null, 
                                bool solid = true)
        :base(
            collider: new CircleCollider(
                position: new Vector2f(0, 0),
                radius: radius,
                elasticity: elasticity,
                friction: friction,
                rotation: rotation
            ),
            drawer: new CircleDrawer(
                _window: window, 
                radius: radius * PhysicsConstants.PixelsPerMeter, 
                color: color,
                solid: solid
            ),
            rotation: rotation
        )
        {
            Radius = radius;
        }
    }
}