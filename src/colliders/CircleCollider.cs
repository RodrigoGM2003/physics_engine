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

            float expandedRadius = Radius * ComputingConstants.ColliderExpansion;
            BoundingBox = new FloatRect(Position.X - expandedRadius, Position.Y - expandedRadius, 2 * expandedRadius, 2 * expandedRadius);
        }

        /**
         * Update the position of the object
         * @param position The new position of the object
         */
        public override void UpdatePosition(Vector2f position){
            Position = position;

            float expandedRadius = Radius * ComputingConstants.ColliderExpansion;
            BoundingBox = new FloatRect(Position.X - expandedRadius, Position.Y - expandedRadius, 2 * expandedRadius, 2 * expandedRadius);
        }


        /**
         * Check if the object intersects with another object
         * @param other The collider to check for intersection
         */
        public override bool Intersects(Collider other){
            return false;
        }
        /**
         * Check if the object intersects with a CircleCollider
         * @param other The collider to check for intersection
         */
        public override bool Intersects(CircleCollider other){
            return false;
        }
        /**
         * Check if the object intersects with a RectangleCollider
         * @param other The collider to check for intersection
         */
        public override bool Intersects(RectangleCollider other){
            return false;
        }
                /**
         * Check if the object intersects with a PolygonCollider
         * @param other The collider to check for intersection
         */
        public override bool Intersects(PolygonCollider other){
            return false;
        }

    }
}