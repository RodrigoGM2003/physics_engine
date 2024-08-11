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

        /**
        * Check if the polygon intersects with a circle
        * @param other The circle collider
        * @param normal The normal of the collision
        * @param depth The depth of the collision
        * @return True if the polygon intersects with the circle, false otherwise
        */
        public override bool Intersects(in CircleCollider other, out Vector2f normal, out float depth)
        {
            normal = new Vector2f(0, 0);
            depth = float.MaxValue;

            // Calculate the true vertices of the polygon
            Vector2f[] thisTrueVertices = CalculateTrueVertices(Vertices, Position, Rotation);

            // Calculate the axes 
            IEnumerable<Vector2f> myAxes = GetAxes(thisTrueVertices);

            // Calculate the closest vertex of the polygon to the circle and the extra axis
            Vector2f closestVertex = GetClosestVertex(other.Position, thisTrueVertices);
            Vector2f extraAxis = closestVertex - other.Position;
            float distance = extraAxis.Length();
            extraAxis /= distance;

            // Add the extra axis to the axes
            myAxes = myAxes.Concat(new Vector2f[]{ extraAxis });

            // Iteate over all axes projecting the vertices of both polygons
            foreach (Vector2f axis in myAxes){
                (float minA, float maxA) = Project(thisTrueVertices, axis);
                (float minB, float maxB) = Project(new Vector2f[]{ other.Position}, axis);

                // //The projection of the circle is a single point, so we need to expand it to both sides
                minB -= other.Radius;
                maxB += other.Radius;

                //Check if the projections overlap if they don't return false
                if (minA >= maxB || minB >= maxA)
                    return false;

                //Calculate the depth of the intersection
                float axisDepth = Math.Min(maxB - minA, maxA - minB);

                if (axisDepth < depth){
                    depth = axisDepth;
                    normal = axis;
                }
            }
            //Normal should always point from the circle to the polygon
            // if (normal.Dot(Position - other.Position) < 0){
            //     // Console.WriteLine("Reversing normal ---------------------------------------------");
            //     normal = -normal;
            // }

            //If all axes overlap, return true
            return true;
        }
        
        /**
        * Check if the polygon intersects with another polygon
        * @param other The other polygon
        * @param normal The normal of the collision
        * @param depth The depth of the collision
        * @return True if the polygons intersect, false otherwise
        * @see https://www.sevenson.com.au/programming/sat/
        * Implementation of the Separating Axis Theorem (SAT)
        */
        public override bool Intersects(in PolygonCollider other, out Vector2f normal, out float depth)
        {
            normal = new Vector2f(0, 0);
            depth = float.MaxValue;
            // Calculate the true vertices of the polygons
            Vector2f[] thisTrueVertices = CalculateTrueVertices(Vertices, Position, Rotation);
            Vector2f[] otherTrueVertices = CalculateTrueVertices(other.Vertices, other.Position, other.Rotation);

            // Calculate the axes of the two polygons
            IEnumerable<Vector2f> myAxes = GetAxes(thisTrueVertices);
            IEnumerable<Vector2f> otherAxes = GetAxes(otherTrueVertices);

            // Iteate over all axes projecting the vertices of both polygons
            foreach (Vector2f axis in myAxes.Concat(otherAxes)){
                (float minA, float maxA) = Project(thisTrueVertices, axis);
                (float minB, float maxB) = Project(otherTrueVertices, axis);

                //Check if the projections overlap if they don't return false
                if (minA >= maxB || minB >= maxA)
                    return false;

                //Calculate the depth of the intersection
                float axisDepth = Math.Min(maxB - minA, maxA - minB);
                if (axisDepth < depth){
                    depth = axisDepth;
                    normal = axis;
                }
            }
            //Normal should always point from the other polygon to this polygon
            // if (normal.Dot(Position - other.Position) < 0){
            //     // Console.WriteLine("Reversing normal ---------------------------------------------");
            //     normal = -normal;
            // }

            //If all axes overlap, return true
            return true;
        }

        /**
        * Calculate the true vertices of the polygon
        * @param vertices The vertices of the polygon
        * @return The true vertices of the polygon
        */
        public Vector2f[] CalculateTrueVertices(Vector2f[] vertices, Vector2f position, float rotation){
            Vector2f[] trueVertices = new Vector2f[vertices.Length];

            //Apply the current position and rotation to the vertices
            float cos = (float)Math.Cos(rotation);
            float sin = (float)Math.Sin(rotation);

            for (int i = 0; i < vertices.Length; i++)
            {
                float x = vertices[i].X;
                float y = vertices[i].Y;

                // Apply the 2D rotation matrix
                trueVertices[i] = new Vector2f(
                    x * cos - y * sin + position.X,  // new x-coordinate
                    x * sin + y * cos + position.Y   // new y-coordinate
                );
            }

            return trueVertices;
        }

        /**
        * Get the axes (normals) of the polygon
        * @param vertices The vertices of the polygon
        * @return The axes of the polygon
        */
        private Vector2f[] GetAxes(Vector2f[] vertices){
            Vector2f[] axes = new Vector2f[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2f edge = vertices[(i + 1) % vertices.Length] - vertices[i];
                axes[i] = new Vector2f(-edge.Y, edge.X);
                axes[i] = axes[i] / axes[i].Length();
            }

            return axes;
        }

        /**
        * Project the vertices of the polygon onto the axis
        * @param vertices The vertices of the polygon
        * @param axis The axis to project the vertices onto
        * @return The min and max projection of the vertices onto the axis
        */
        private (float, float) Project(Vector2f[] vertices, Vector2f axis){
            float min = float.MaxValue, max = float.MinValue;

            foreach (Vector2f vertex in vertices)
            {
                //Project the vertices onto the axis (projection = dot product of the vertex and the axis)
                float projection = axis.Dot(vertex);
                if (projection < min)
                    min = projection;
                if (projection > max)
                    max = projection;
            }

            return (min, max);
        }

        /**
        * Get the closest vertex of the polygon to a point
        * @param point The point in space
        * @param vertices The vertices of the polygon
        * @return The closest vertex of the polygon to the point
        */
        private Vector2f GetClosestVertex(Vector2f point, Vector2f[] vertices)
        {
            Vector2f closestVertex = vertices[0];
            float minDistance = (closestVertex - point).LengthSquared();

            foreach (Vector2f vertex in vertices)
            {
                float distance = (vertex - point).LengthSquared();
                if (distance < minDistance)
                {
                    closestVertex = vertex;
                    minDistance = distance;
                }
            }
            //Vertex and index of the closest vertex
            return closestVertex;
        }

        /**
        * Get the center of the polygon
        * @param vertices The vertices of the polygon
        * @return The center of the polygon
        */
        private Vector2f GetCenter(Vector2f[] vertices){
            Vector2f center = new Vector2f(0, 0);

            foreach (Vector2f vertex in vertices)
                center += vertex;
            
            return center / vertices.Length;
        }
    }
}