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
            LastPosition = Position;
            Position = position;

            BoundingBox = new FloatRect(Position.X - Size.X / 2, Position.Y - Size.Y / 2, Size.X, Size.Y);
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