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
        private float maxDistance;

        /**
        * Base constructor for PolygonCollider class
        * @param position The position of the object in the scene
        * @param vertices The vertices of the polygon
        * @param rotation The rotation of the object
        * @param elasticity The elasticity of the object
        * @param friction The friction of the object
        */
        public PolygonCollider(Vector2f position, in Vector2f[] vertices, float? rotation = 0, float? elasticity = null, float? friction = null)
        : base(position: position, 
            elasticity: elasticity, 
            friction: friction, 
            rotation: rotation
        )
        {
            Vertices = vertices;

            //Apply the rotation to the vertices
            if (rotation != 0 && rotation != null){
                float cos = (float)Math.Cos(rotation.Value);
                float sin = (float)Math.Sin(rotation.Value);

                for (int i = 0; i < vertices.Length; i++)
                {
                    float x = vertices[i].X;
                    float y = vertices[i].Y;
                    vertices[i] = new Vector2f(x * cos - y * sin, x * sin + y * cos);
                }
            }

            //Calculate the largest distance from a vertex to the center
            maxDistance = 0;
            foreach (Vector2f vertex in Vertices){
                float distance = (float)Math.Sqrt(Math.Pow(vertex.X, 2) + Math.Pow(vertex.Y, 2));
                if (distance > maxDistance)
                    maxDistance = distance;
            }

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

            // maxDistance = (float)Math.Sqrt(Math.Pow(maxX - minX, 2) + Math.Pow(maxY - minY, 2));

            //Expand the bounding box
            float width = maxX - minX;
            float height = maxY - minY;
            // BoundingBox = new FloatRect(Position.X - width / 2, Position.Y - height / 2, width, height);
            BoundingBox = new FloatRect(Position.X - maxDistance, Position.Y - maxDistance, maxDistance, maxDistance);
        }

        /**
        * Update the position of the object
        * @param position The new position of the object
        */
        public override void UpdatePosition(Vector2f position, float rotation){
            LastPosition = Position;
            Position = position;

            LastRotation = Rotation;
            Rotation = rotation;

            //Apply the rotation to the vertices
            if (rotation != 0){
                float cos = (float)Math.Cos(rotation);
                float sin = (float)Math.Sin(rotation);

                for (int i = 0; i < Vertices.Length; i++)
                {
                    float x = Vertices[i].X;
                    float y = Vertices[i].Y;
                    Vertices[i] = new Vector2f(x * cos - y * sin, x * sin + y * cos);
                }
            }

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
            float width = maxX - minX;
            float height = maxY - minY;
            // BoundingBox = new FloatRect(Position.X - width / 2, Position.Y - height / 2, width, height);
            BoundingBox = new FloatRect(Position.X - maxDistance, Position.Y - maxDistance, maxDistance, maxDistance);
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