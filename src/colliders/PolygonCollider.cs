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

            UpdateBoundingBox();
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

            UpdateBoundingBox();
        }

        /**
        * Update the bounding box of the object
        */
        private void UpdateBoundingBox(){
            Vector2f[] newVertices = new Vector2f[Vertices.Length];

            //Apply the rotation to the vertices
            float cos = (float)Math.Cos(Rotation);
            float sin = (float)Math.Sin(Rotation);

            for (int i = 0; i < Vertices.Length; i++)
            {
                float x = Vertices[i].X;
                float y = Vertices[i].Y;

                // Apply the 2D rotation matrix
                newVertices[i] = new Vector2f(
                    x * cos - y * sin,  // new x-coordinate
                    x * sin + y * cos   // new y-coordinate
                );
            }
            
            //Calculate the bounding box
            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;

            //Calculate AABB
            foreach (Vector2f vertex in newVertices){
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
            BoundingBox = new FloatRect(Position.X - width / 2, Position.Y - height / 2, width, height);
        }
    }
}