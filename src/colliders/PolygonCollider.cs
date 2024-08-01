using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Data;
using System.Numerics;


namespace PhysicsEngine
{   
    /**
     * Class for all circle colliders in the simulation
     */ 
    public class PolygonCollider : Collider
    {
        public Vector2f[] Vertices { get; protected set; } // The vertices of the polygon

        /**
         * Base constructor for PolygonCollider class
         * @param position The position of the object in the scene
         * @param radius The radius of the object
         * @param elasticity The elasticity of the object
         * @param friction The friction of the object
         */
        public PolygonCollider(Vector2f position, Vector2f[] vertices, float? elasticity = null, float? friction = null)
        : base(position, elasticity, friction)
        {
            Vertices = vertices;

            //Calculate the bounding box
            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;

            //Calculate AABB
            foreach (Vector2f vertex in Vertices){
                if (vertex.X < minX)
                    minX = vertex.X;
                if (vertex.X > maxX)
                    maxX = vertex.X;
                if (vertex.Y < minY)
                    minY = vertex.Y;
                if (vertex.Y > maxY)
                    maxY = vertex.Y;
            }

            //Expand the bounding box
            float expandedWidth = (maxX - minX) * ComputingConstants.ColliderExpansion;
            float expandedHeight = (maxY - minY) * ComputingConstants.ColliderExpansion;
            BoundingBox = new FloatRect(minX - expandedWidth / 2, minY - expandedHeight / 2, expandedWidth, expandedHeight);
        }

        /**
         * Update the position of the object
         * @param position The new position of the object
         */
        public override void UpdatePosition(Vector2f position){
            Position = position;

            //Calculate the bounding box
            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;

            //Calculate AABB
            foreach (Vector2f vertex in Vertices){
                if (vertex.X < minX)
                    minX = vertex.X;
                if (vertex.X > maxX)
                    maxX = vertex.X;
                if (vertex.Y < minY)
                    minY = vertex.Y;
                if (vertex.Y > maxY)
                    maxY = vertex.Y;
            }

            //Expand the bounding box
            float expandedWidth = (maxX - minX) * ComputingConstants.ColliderExpansion;
            float expandedHeight = (maxY - minY) * ComputingConstants.ColliderExpansion;
            BoundingBox = new FloatRect(minX - expandedWidth / 2, minY - expandedHeight / 2, expandedWidth, expandedHeight);
        }

        /**
         * Check if the object intersects with another object
         * @param other The collider to check for intersection
         */
        public override bool Intersects(Collider other){
            return false;
        }
        /**
         * Check if the object intersects with a PolygonCollider
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