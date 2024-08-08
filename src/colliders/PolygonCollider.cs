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

        public override bool Intersects(in CircleCollider other)
        {
            IEnumerable<Vector2f> myAxes = GetAxes(Vertices.Select(v => v + this.Position).ToArray());
            IEnumerable<Vector2f> otherAxes = GetAxes(new Vector2f[]{ GetClosestVertex(other.Position, Vertices)});

            foreach (Vector2f axis in myAxes.Concat(otherAxes)){
                (float minA, float maxA) = Project(Vertices, axis);
                (float minB, float maxB) = Project(new Vector2f[] { other.Position }, axis);

                minB -= other.Radius;
                maxB += other.Radius;

                if (maxA < minB || maxB < minA)
                    return false;
                
            }
            return true;
        }
        /**
        * Check if the polygon intersects with another polygon
        * @param other The other polygon
        * @return True if the polygons intersect, false otherwise
        * @see https://www.sevenson.com.au/programming/sat/
        * Implementation of the Separating Axis Theorem (SAT)
        */
        public override bool Intersects(in PolygonCollider other)
        {
            Vector2f[] thisTrueVertices = new Vector2f[Vertices.Length];
            Vector2f[] otherTrueVertices = new Vector2f[other.Vertices.Length];

            //Apply the current position and rotation to the vertices
            float cos = (float)Math.Cos(Rotation);
            float sin = (float)Math.Sin(Rotation);

            for (int i = 0; i < Vertices.Length; i++)
            {
                float x = Vertices[i].X;
                float y = Vertices[i].Y;

                // Apply the 2D rotation matrix
                thisTrueVertices[i] = new Vector2f(
                    x * cos - y * sin + Position.X,  // new x-coordinate
                    x * sin + y * cos + Position.Y   // new y-coordinate
                );
            }

            cos = (float)Math.Cos(other.Rotation);
            sin = (float)Math.Sin(other.Rotation);

            for (int i = 0; i < other.Vertices.Length; i++)
            {
                float x = other.Vertices[i].X;
                float y = other.Vertices[i].Y;

                // Apply the 2D rotation matrix
                otherTrueVertices[i] = new Vector2f(
                    x * cos - y * sin + other.Position.X,  // new x-coordinate
                    x * sin + y * cos + other.Position.Y   // new y-coordinate
                );
            }
            //Calculate the axes of the two polygons
            // IEnumerable<Vector2f> myAxes = GetAxes(Vertices);
            // IEnumerable<Vector2f> otherAxes = GetAxes(other.Vertices);
            IEnumerable<Vector2f> myAxes = GetAxes(thisTrueVertices);
            IEnumerable<Vector2f> otherAxes = GetAxes(otherTrueVertices);

            Console.WriteLine("A position: " + Position.X + " " + Position.Y);
            Console.WriteLine("A true 1 vertex: + " + thisTrueVertices[0].X + " " + thisTrueVertices[0].Y);
            Console.WriteLine("A true 2 vertex: + " + thisTrueVertices[1].X + " " + thisTrueVertices[1].Y);
            Console.WriteLine("A true 3 vertex: + " + thisTrueVertices[2].X + " " + thisTrueVertices[2].Y);
            Console.WriteLine("A true 4 vertex: + " + thisTrueVertices[3].X + " " + thisTrueVertices[3].Y);
            Console.WriteLine("B position: " + other.Position.X + " " + other.Position.Y);
            Console.WriteLine("B true 1 vertex: + " + otherTrueVertices[0].X + " " + otherTrueVertices[0].Y);
            Console.WriteLine("B true 2 vertex: + " + otherTrueVertices[1].X + " " + otherTrueVertices[1].Y);
            Console.WriteLine("B true 3 vertex: + " + otherTrueVertices[2].X + " " + otherTrueVertices[2].Y);
            Console.WriteLine("B true 4 vertex: + " + otherTrueVertices[3].X + " " + otherTrueVertices[3].Y);


            //Iteate over all axes projecting the vertices of both polygons
            foreach (Vector2f axis in myAxes.Concat(otherAxes)){
                (float minA, float maxA) = Project(thisTrueVertices, axis);
                (float minB, float maxB) = Project(otherTrueVertices, axis);

                //Check if the projections overlap if they don't return false
                if (minA > maxB || minB > maxA){
                    Console.WriteLine("Polygon does not intersect with polygon " + minA + " " + maxA + " / " + minB + " " + maxB);
                    return false;
                }
            }

            Console.WriteLine("Polygon intersects with polygon");
            //If all axes overlap, return true
            return true;
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
                //Calculate the normal of the edge (normal = axis)
                Vector2f p1 = vertices[i];
                Vector2f p2 = vertices[(i + 1) % vertices.Length];

                Vector2f edge = p2 - p1;
                Vector2f normal = new Vector2f(-edge.Y, edge.X);
                normal = normal / normal.Length();

                axes[i] = normal;
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

            return closestVertex;
        }
    }
}