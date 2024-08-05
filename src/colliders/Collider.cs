using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;
using System.Numerics;


namespace PhysicsEngine
{   
    /**
     * Abstract class for all colliders in the simulation
     */ 
    public abstract class Collider
    {
        public Vector2f Position { get; protected set; } // Position in m
        public Vector2f LastPosition { get; protected set; } // Last position in m (used to calculate swept AABB)
        public float Rotation { get; protected set; } // Rotation in rad
        public float LastRotation { get; protected set; } // Last rotation in rad (used to calculate swept AABB)
        public float Elasticity { get; protected set; } // The elasticity of the object
        public float Friction { get; protected set; } // The friction of the object
        public FloatRect BoundingBox { get; protected set; } // The bounding box of the object (used to increase performance in collision detection)
        public FloatRect SweptAABB { get; protected set; } // The swept AABB of the object (used to calculate collision response)

        /**
         * Base constructor for the Collider class
         * @param position The position of the object in the scene
         * @param elasticity The elasticity of the object
         * @param friction The friction of the object
         */
        protected Collider(Vector2f position, float? rotation = 0, float? elasticity = null, float? friction = null)
        {
            Position = position;
            LastPosition = position;

            Rotation = rotation ?? 0;
            LastRotation = rotation ?? 0;
            
            Elasticity = elasticity ?? PhysicsConstants.DefaultElasticity;
            Friction = friction ?? PhysicsConstants.DefaultFriction;
        }

        /**
         * Update the position of the object
         * @param position The new position of the object
         */
        public abstract void UpdatePosition(Vector2f position, float rotation);
        

        /**
         * Update the swept AABB of the object
         */
        public void UpdateSweptAABB(){

            float minX = Math.Min(Position.X - BoundingBox.Width / 2, LastPosition.X - BoundingBox.Width / 2);
            float minY = Math.Min(Position.Y - BoundingBox.Height / 2, LastPosition.Y - BoundingBox.Height / 2);
            float maxX = Math.Max(Position.X + BoundingBox.Width / 2, LastPosition.X + BoundingBox.Width / 2);
            float maxY = Math.Max(Position.Y + BoundingBox.Height / 2, LastPosition.Y + BoundingBox.Height / 2);

            SweptAABB = new FloatRect(minX, minY, maxX - minX, maxY - minY);
        }

        /**
         * Resolve the collision between the object and another object
         * @param other The collider to resolve the collision with
         */
        public abstract void ResolveCollision(in CircleCollider other);
        public abstract void ResolveCollision(in RectangleCollider other);
        public abstract void ResolveCollision(in PolygonCollider other);

    }
}