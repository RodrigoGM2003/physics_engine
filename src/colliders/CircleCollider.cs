using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;


namespace PhysicsEngine
{   
    /**
     * Class for all circle colliders in the simulation
     */ 
    public class CircleCollider : Collider
    {
        public float Radius { get; protected set; } // The radius of the circle

        /**
         * Base constructor for CircleCollider class
         * @param position The position of the object in the scene
         * @param radius The radius of the object
         * @param elasticity The elasticity of the object
         * @param friction The friction of the object
         */
        public CircleCollider(Vector2f position, float radius, float? elasticity = null, float? friction = null)
        : base(position, elasticity, friction)
        {
            Radius = radius;

            BoundingBox = new FloatRect(Position.X - Radius, Position.Y - Radius, 2 * Radius, 2 * Radius);        
        }

        /**
         * Update the position of the object
         * @param position The new position of the object
         */
        public override void UpdatePosition(Vector2f position){
            LastPosition = Position;
            Position = position;

            BoundingBox = new FloatRect(Position.X - Radius, Position.Y - Radius, 2 * Radius, 2 * Radius);        
        }


        /**
         * Resolve the collision between the object and another object
         * @param other The collider to resolve the collision with
         */
        public override void ResolveCollision(in CircleCollider other){

        }
        public override void ResolveCollision(in RectangleCollider other){

        }
        public override void ResolveCollision(in PolygonCollider other){

        }

    }
}