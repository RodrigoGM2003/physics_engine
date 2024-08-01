using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;


namespace PhysicsEngine
{   
    /**
     * Abstract class for all colliders in the simulation
     */ 
    public abstract class Collider
    {
        public Vector2f Position { get; protected set; } // Position in m
        public float Elasticity { get; protected set; } // The elasticity of the object
        public float Friction { get; protected set; } // The friction of the object
        public FloatRect BoundingBox { get; protected set; } // The bounding box of the object (used to increase performance in collision detection)


        /**
         * Base constructor for the Collider class
         * @param position The position of the object in the scene
         * @param elasticity The elasticity of the object
         * @param friction The friction of the object
         */
        protected Collider(Vector2f position, float? elasticity = null, float? friction = null)
        {
            Position = position;

            if (elasticity.HasValue)
                Elasticity = elasticity.Value;
            else
                Elasticity = PhysicsConstants.DefaultElasticity;

            if (friction.HasValue)
                Friction = friction.Value;
            else
                Friction = PhysicsConstants.DefaultFriction;
        }

        /**
         * Update the position of the object
         * @param position The new position of the object
         */
        public abstract void UpdatePosition(Vector2f position);

        /**
         * Check if the object intersects with another object
         * @param other The collider to check for intersection
         */
        public abstract bool Intersects(Collider other);
        /**
         * Check if the object intersects with a CircleCollider
         * @param other The collider to check for intersection
         */
        public abstract bool Intersects(CircleCollider other);
        /**
         * Check if the object intersects with a RectangleCollider
         * @param other The collider to check for intersection
         */
        public abstract bool Intersects(RectangleCollider other);
                /**
         * Check if the object intersects with a PolygonCollider
         * @param other The collider to check for intersection
         */
        public abstract bool Intersects(PolygonCollider other);

    }
}