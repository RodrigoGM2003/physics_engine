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
    public class RectangleCollider : Collider
    {
        public Vector2f Size { get; protected set; } // The size of the rectangle

        /**
         * Base constructor for RectangleCollider class
         * @param position The position of the object in the scene
         * @param radius The radius of the object
         * @param elasticity The elasticity of the object
         * @param friction The friction of the object
         */
        public RectangleCollider(Vector2f position, Vector2f size, float? elasticity = null, float? friction = null)
        : base(position, elasticity, friction)
        {
            Size = Size;
            float expandedWidth = Size.X * ComputingConstants.ColliderExpansion;
            float expandedHeight = Size.Y * ComputingConstants.ColliderExpansion ;
            BoundingBox = new FloatRect(Position.X - expandedWidth / 2, Position.Y - expandedHeight / 2, expandedWidth, expandedHeight);
        }

        /**
         * Update the position of the object
         * @param position The new position of the object
         */
        public override void UpdatePosition(Vector2f position){
            Position = position;
            float expandedWidth = Size.X * ComputingConstants.ColliderExpansion;
            float expandedHeight = Size.Y * ComputingConstants.ColliderExpansion;
            BoundingBox = new FloatRect(Position.X - expandedWidth / 2, Position.Y - expandedHeight / 2, expandedWidth, expandedHeight);
        }


        /**
         * Check if the object intersects with another object
         * @param other The collider to check for intersection
         */
        public override bool Intersects(Collider other){
            return false;
        }
        /**
         * Check if the object intersects with a RectangleCollider
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